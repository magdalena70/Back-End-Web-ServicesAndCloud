namespace BugTracker.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public class Bug
    {
        public Bug()
        {
            this.Comments = new HashSet<Comment>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Title { get; set; }

        public string Description  { get; set; }

        [Required]
        public BugStatus Status  { get; set; }

        public string AuthorId { get; set; }

        public virtual User Author  { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
