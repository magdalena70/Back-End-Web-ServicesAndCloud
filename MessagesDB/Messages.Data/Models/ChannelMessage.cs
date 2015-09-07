using System;
using System.ComponentModel.DataAnnotations;
namespace Messages.Data.Models
{
    public class ChannelMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public DateTime DateSent { get; set; }

        public string SenderId { get; set; }

        public virtual User Sender { get; set; }

        public int ChannelId { get; set; }

        [Required]
        public virtual Channel Channel { get; set; }
    }
}
