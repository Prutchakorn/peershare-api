using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PeerShareV2.Models
{
    public class UserAccount
    {
        [Key]
        public long UserId { get; set; }

        [StringLength(20)]
        public String FirstName { get; set; } = "";

        [StringLength(20)]
        public String LastName { get; set; } = "";
        
        [Required]
        public String Password { get; set; }

        [Required]
        [StringLength(13)]
        public String PromptPay { get; set; }

        [DisplayFormat(DataFormatString = "dd'/'MM'/'yyyy")]
        public DateTime BirthDate { get; set; }
        public String ProfileImageUrl { get; set; } = "";
    }
}