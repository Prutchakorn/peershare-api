using System;
using System.ComponentModel.DataAnnotations;

namespace PeerShareV2.Models
{
    public class Status
    {
        [Key]
        public long Id { get; set; }
        
        [Required]
        [StringLength(20)]
        public String Name { get; set; }
        
        [StringLength(200)]
        public String Description { get; set; }
        
        [StringLength(7)]
        public String ColorCode { get; set; }
    }
}