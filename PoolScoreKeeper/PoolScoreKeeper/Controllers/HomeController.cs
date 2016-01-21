using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PoolScoreKeeper.Models;
using Raven.Client;

namespace PoolScoreKeeper.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDocumentStore _store = RavenDb.GetStore();

        public ActionResult Index()
        {
            //Seed();

            List<Player> players;
            int[,] scores;
            using (var session = _store.OpenSession())
            {
                players = session.Query<Player>().OrderBy(x=>x.FirstName).ToList();
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

            var model = new HomeIndexViewModel
            {
                Players = players,
                Scores = scores
            };

            return View(model);
        }

        public JsonResult RegisterGame(PoolGame poolGame)
        {
            if (poolGame.WinnerId == poolGame.LoserId)
            {
                return Json("Winner and loser can't be same person", JsonRequestBehavior.AllowGet);
            }

            poolGame.CreatedDate = DateTime.Now;
            using (var session = _store.OpenSession())
            {
                session.Store(poolGame);
                session.SaveChanges();
            }

            return Json("Game Registered succesfull", JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private void Seed()
        {
            using (var session = _store.OpenSession())
            {
                var players = new List<Player>
                {
                    new Player {FirstName = "Vikiet", LastName = "Chung", NickName = "Kitkat"},
                    new Player {FirstName = "Armin", LastName = "Vojnikovic", NickName = "Magic"},
                    new Player {FirstName = "Acke", LastName = "Salem", NickName = "Big Boss"},
                    new Player {FirstName = "Tore", LastName = "Nestenius", NickName = "The Educator"},
                    new Player {FirstName = "Paul", LastName = "Histrand", NickName = "Cheater"},
                    new Player {FirstName = "Ammar", LastName = "Hijjo", NickName = "Mr Handy"},
                    new Player {FirstName = "Eric", LastName = "Lavesson", NickName = "Decemberborn"},
                    new Player {FirstName = "Sheldon", LastName = "Keeping", NickName = "Big Bang Theory"},
                    new Player {FirstName = "Björn", LastName = "Lindell", NickName = "Scrum Master"},
                    new Player {FirstName = "Erik", LastName = "Man", NickName = "The Man"},
                };
                players.ForEach(session.Store);
                session.SaveChanges();

                //var random = new Random();
                //int playerCount = players.Count;
                //var poolGames = new List<PoolGame>();

                //for (int i = 0; i < 200; i++)
                //{
                //    int winner = random.Next(0, playerCount);
                //    int loser = random.Next(0, playerCount);
                //    while (loser == winner)
                //    {
                //        loser = random.Next(0, playerCount);
                //    }

                //    poolGames.Add(new PoolGame
                //    {
                //        WinnerId = players[winner].Id,
                //        LoserId = players[loser].Id,
                //        CreatedDate = DateTime.Now.AddDays(0 - random.Next(90)),
                //        WinnerPocketedEightball = random.Next(2) % 2 == 0
                //    });
                //}
                //poolGames.ForEach(session.Store);
                //session.SaveChanges();
            }
        }
    }
}