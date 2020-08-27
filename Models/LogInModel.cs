using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeerShareV2.Models
{
    [NotMapped]
    public class LogInModel
    {
        [StringLength(13)]
        public string PromptPay { get; set; }
        public string Password { get; set; }
    }
}