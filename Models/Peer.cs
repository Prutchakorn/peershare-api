using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace PeerShareV2.Models
{
    public class Peer
    {
        public long Id { get; set; }
        public long BillSplitId { get; set; }
        
        [Required]
        [StringLength(50)]
        public String Name { get; set; }
        
        [Required]
        [StringLength(13)]
        public String PromptPay { get; set; }
        public Boolean IsPromptPay { get; set; } = true;
        
        [Required]
        public Double PersonalTotalPrice { get; set; }
        public Double PersonalNetPrice { get; set; } = 0.0;
        
        [Required]
        // status 2 = 'payment pending'
        public long StatusId { get; set; } = 2;
        
        [DisplayFormat(DataFormatString = "dd'/'MM'/'yyyy HH:mm")]
        public DateTime PaidDateTime { get; set; }
        public Boolean IsActive { get; set; } = true;
        public long? UserAccountId { get; set; }

        [ForeignKey("UserAccountId")]
        public virtual UserAccount UserAccount { get; set; }
        
        [ForeignKey("StatusId")]
        public virtual Status Status { get; set; }
    }
}