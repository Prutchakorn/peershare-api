using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeerShareV2.Models
{
    public class BillSplit
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(13)]
        public String OwnerPromptPay { get; set; }
        
        [Required]
        public Double TotalPrice { get; set; }
        
        [Required]
        // including the owner
        public int NumberOfPeople { get; set; }
        
        [Required]
        public DateTime CreatedDateTime { get; set; }
        
        // 7% store as 7
        public Double Vat { get; set; } = 0.0;
        public Double ServiceCharge { get; set; } = 0.0;

        [StringLength(200)]
        public String Name { get; set; }

        [Required]
        public Boolean IsActive { get; set; } = true;
        
        [NotMapped]
        public List<Peer> Peers { get; set; }
    }
}