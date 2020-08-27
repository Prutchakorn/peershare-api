using System;
using System.ComponentModel.DataAnnotations;

namespace PeerShareV2.Models
{
    public class RTPModel
    {
        [Key]
        public long Id { get; set; }
        
        [Required]
        [StringLength(13)]
        public string SenderPromptPay { get; set; }

        [Required]
        [StringLength(13)]
        public string ReceiverPromptPay { get; set; }
        
        [Required]
        public DateTime RequestedDateTime { get; set; }
        
        [Required]
        // In minutes
        public int ActivePeriod { get; set; } = 5;
        
        [Required]
        public double Amount { get; set; }
        public Boolean IsActive { get; set; } = true;
    }
}