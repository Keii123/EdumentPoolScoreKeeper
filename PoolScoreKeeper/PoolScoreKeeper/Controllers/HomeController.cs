using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PoolScoreKeeper.Interfaces;
using PoolScoreKeeper.Models;
using Raven.Client;

namespace PoolScoreKeeper.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPoolOperations poolOps;

        public HomeController(IPoolOperations poolOps)
        {
            this.poolOps = poolOps;
        }

        public ActionResult Index()
        {
            return View(poolOps.GetScoreBoard());
        }

        public JsonResult RegisterGame(PoolGame poolGame)
        {
            if (poolGame.WinnerId == poolGame.LoserId)
                return Json("Winner and loser can't be the same person", JsonRequestBehavior.AllowGet);

            poolOps.RegisterGame(poolGame);

            return Json("Game Registered successfully", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPlayerStatistics(string playerId)
        {
            var playerStatistics = poolOps.GetPlayerStatistics(playerId);

            return Json(playerStatistics, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PlayerComparison(string winnerSidePlayerId, string runnerUpSidePlayerId)
        {
            var comparePlayersStatistics = poolOps.GetComparePlayersStatistics(winnerSidePlayerId, runnerUpSidePlayerId);
            return View(comparePlayersStatistics);
        }

        //private void Seed()
        //{
        //    using (var session = _store.OpenSession())
        //    {
        //        var players = new List<Player>
        //        {
        //            new Player {FirstName = "Vikiet", LastName = "Chung", NickName = "Kitkat"},
        //            new Player {FirstName = "Armin", LastName = "Vojnikovic", NickName = "Magic"},
        //            new Player {FirstName = "Acke", LastName = "Salem", NickName = "Big Boss"},
        //            new Player {FirstName = "Tore", LastName = "Nestenius", NickName = "The Educator"},
        //            new Player {FirstName = "Paul", LastName = "Histrand", NickName = "Cheater"},
        //            new Player {FirstName = "Ammar", LastName = "Hijjo", NickName = "Mr Handy"},
        //            new Player {FirstName = "Eric", LastName = "Lavesson", NickName = "Decemberborn"},
        //            new Player {FirstName = "Sheldon", LastName = "Keeping", NickName = "Big Bang Theory"},
        //            new Player {FirstName = "Björn", LastName = "Lindell", NickName = "Scrum Master"},
        //            new Player {FirstName = "Erik", LastName = "Man", NickName = "The Man"},
        //        };
        //        players.ForEach(session.Store);
        //        session.SaveChanges();

        //        //var random = new Random();
        //        //int playerCount = players.Count;
        //        //var poolGames = new List<PoolGame>();

        //        //for (int i = 0; i < 200; i++)
        //        //{
        //        //    int winner = random.Next(0, playerCount);
        //        //    int loser = random.Next(0, playerCount);
        //        //    while (loser == winner)
        //        //    {
        //        //        loser = random.Next(0, playerCount);
        //        //    }

        //        //    poolGames.Add(new PoolGame
        //        //    {
        //        //        WinnerId = players[winner].Id,
        //        //        LoserId = players[loser].Id,
        //        //        CreatedDate = DateTime.Now.AddDays(0 - random.Next(90)),
        //        //        WinnerPocketedEightball = random.Next(2) % 2 == 0
        //        //    });
        //        //}
        //        //poolGames.ForEach(session.Store);
        //        //session.SaveChanges();
        //    }
        //}
    }
}