using System;
using System.ComponentModel.DataAnnotations;
namespace Messages.Data.Models
{
    public class UserMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public DateTime DateSent { get; set; }

        public string SenderId { get; set; }

        public virtual User Sender { get; set; }

        public string RecipientId { get; set; }

        [Required]
        public virtual User Recipient { get; set; }
    }
}
