using System;

namespace PoolScoreKeeper.Models
{
    public class PoolGame
    {
        public string Id { get; set; }
        public string WinnerId { get; set; }
        public string LoserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool WinnerPocketedEightball { get; set; }
    }
}