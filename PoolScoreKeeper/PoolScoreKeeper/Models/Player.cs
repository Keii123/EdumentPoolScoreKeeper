using System.ComponentModel.DataAnnotations;

namespace PoolScoreKeeper.Models
{
    public class Player
    {
        public string Id { get; set; }
        [Required]
        [RegularExpression("^[A-Za-zåäöÅÄÖ]{1,25}$", ErrorMessage = "Only swedish letters, no spaces")]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression("^[A-Za-zåäöÅÄÖ]{1,25}$", ErrorMessage = "Only swedish letters, no spaces")]
        public string LastName { get; set; }
        [Required]
        [RegularExpression("^[A-Za-zåäöÅÄÖ ]{1,25}$", ErrorMessage = "Only swedish letters")]
        public string NickName { get; set; }
    }
}