using System;
using System.Collections.Generic;
using System.Linq;
using PoolScoreKeeper.Interfaces;
using PoolScoreKeeper.Models;
using Raven.Client;

namespace PoolScoreKeeper.Services
{
    public class PoolOperations : IPoolOperations
    {
        private readonly IDocumentStore store = RavenDb.GetStore();

        public ScoreBoard GetScoreBoard()
        {
            List<Player> players;
            int[,] scores;
            using (var session = store.OpenSession())
            {
                players = session.Query<Player>().OrderBy(x => x.FirstName).ToList();
                List<PoolGame> poolGames = session.Query<PoolGame>().Take(10000).ToList();

                scores = new int[players.Count, players.Count];
                for (int i = 0; i < players.Count; i++)
                {
                    for (int j = 0; j < players.Count; j++)
                    {
                        if (i == j)
                            continue;

                        scores[i, j] = poolGames.Count(x => x.WinnerId == players[i].Id && x.LoserId == players[j].Id);
                    }
                }
            }

            return new ScoreBoard
            {
                Players = players,
                Scores = scores
            };
        }

        public PlayerStatistics GetPlayerStatistics(string playerId)
        {
            using (var session = store.OpenSession())
            {
                var player = session.Load<Player>(playerId);
                var poolGames = session.Query<PoolGame>()
                    .Where(x => x.LoserId == playerId || x.WinnerId == playerId).ToList();

                var losses = poolGames.Count(x => x.LoserId == playerId);
                var wins = poolGames.Count(x => x.WinnerId == playerId);

                return new PlayerStatistics
                {
                    Id = playerId,
                    Name = player.FirstName + " " + player.LastName,
                    Losses = losses,
                    Wins = wins
                };
            }
        }

        public void RegisterGame(PoolGame poolGame)
        {
            poolGame.CreatedDate = DateTime.Now;
            using (var session = store.OpenSession())
            {
                session.Store(poolGame);
                session.SaveChanges();
            }
        }

        public ComparePlayersStatistics GetComparePlayersStatistics(string winnerSidePlayerId, string runnerUpSidePlayerId)
        {
            using (var session = store.OpenSession())
            {
                var winnerSidePlayer = session.Load<Player>(winnerSidePlayerId);
                var runnerUpSidePlayer = session.Load<Player>(runnerUpSidePlayerId);
                var poolGames = session.Query<PoolGame>().Where(x => (x.WinnerId == winnerSidePlayerId || x.WinnerId == runnerUpSidePlayerId) && 
                                                                     (x.LoserId == winnerSidePlayerId || x.LoserId == runnerUpSidePlayerId)).ToList();

                var winnerSideWins = poolGames.Count(x => x.WinnerId == winnerSidePlayerId);
                var runnerUpSideWins = poolGames.Count(x => x.WinnerId == runnerUpSidePlayerId);
                var winnerSideWinningEightBalls = poolGames.Count(x => x.WinnerId == winnerSidePlayerId && x.WinnerPocketedEightball);
                var winnerSideLosingEightBalls = poolGames.Count(x => x.LoserId == winnerSidePlayerId && !x.WinnerPocketedEightball);
                var runnerUpSideWinningEightBalls = poolGames.Count(x => x.WinnerId == runnerUpSidePlayerId && x.WinnerPocketedEightball);
                var runnerUpSideLosingEightBalls = poolGames.Count(x => x.LoserId == runnerUpSidePlayerId && !x.WinnerPocketedEightball);

                return new ComparePlayersStatistics
                {
                    WinningSidePlayer = new PlayerStatistics
                    {
                        Name = winnerSidePlayer.FirstName + " " + winnerSidePlayer.LastName,
                        Wins = winnerSideWins,
                        Losses = runnerUpSideWins,
                        WinningEightBalls = winnerSideWinningEightBalls,
                        LosingEightBalls = winnerSideLosingEightBalls
                    },
                    RunnerUpSidePlayer = new PlayerStatistics
                    {
                        Name = runnerUpSidePlayer.FirstName + " " + runnerUpSidePlayer.LastName,
                        Wins = runnerUpSideWins,
                        Losses = winnerSideWins,
                        WinningEightBalls = runnerUpSideWinningEightBalls,
                        LosingEightBalls = runnerUpSideLosingEightBalls
                    }

                };
            }
        }
    }
}