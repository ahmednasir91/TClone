using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TwitterClone.Entities
{
    public enum Privacy
    {
        Public,
        Private
    }

    public class List
    {
        [Key]
        public Int64 ID { get; set; }
        [Required]
        public string ListName { get; set; }
        public string Description { get; set; }
        public Privacy Privacy { get; set; }
        [ForeignKey("Username")]
        public User User { get; set; }
        [ForeignKey("User")]
        public string Username { get; set; }
        public ICollection<User> Members { get; set; }
    }
}