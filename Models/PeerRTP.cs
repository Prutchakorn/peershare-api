using System.ComponentModel.DataAnnotations.Schema;

namespace PeerShareV2.Models
{
    public class PeerRTP
    {
        public long PeerId { get; set; }
        public long RTPId { get; set; }

        [ForeignKey("PeerId")]
        public virtual Peer Peer { get; set; }
        
        [ForeignKey("RTPId")]
        public virtual RTPModel RTPModel { get; set; }
    }
}