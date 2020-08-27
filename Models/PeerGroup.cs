using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PeerShareV2.Models
{
    public class PeerGroup
    {
        [Key]
        public long Id { get; set; }
        public List<UserAccount> Members { get; set; }
        
        [StringLength(20)]
        public string Name { get; set; }
    }
}