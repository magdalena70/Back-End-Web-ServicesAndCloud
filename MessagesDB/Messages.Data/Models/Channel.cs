using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Messages.Data.Models
{
    public class Channel
    {
        public Channel()
        {
            this.ChannelMessages = new HashSet<ChannelMessage>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }

        public virtual ICollection<ChannelMessage> ChannelMessages { get; set; }

       
    }
}
