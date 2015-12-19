using System.Collections.Generic;

namespace PoolScoreKeeper.Models
{
    public class HomeIndexViewModel
    {
        public List<Player> Players { get; set; }
        public int[,] Scores { get; set; }
    }
}