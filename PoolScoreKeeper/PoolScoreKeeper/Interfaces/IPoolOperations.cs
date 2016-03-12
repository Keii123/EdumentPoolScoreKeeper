using PoolScoreKeeper.Models;

namespace PoolScoreKeeper.Interfaces
{
    public interface IPoolOperations
    {
        ScoreBoard GetScoreBoard();

        PlayerStatistics GetPlayerStatistics(string playerId);

        void RegisterGame(PoolGame poolGame);

        ComparePlayersStatistics GetComparePlayersStatistics(string winnerSidePlayerId, string runnerUpSidePlayerId);
    }
}