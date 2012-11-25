using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TwitterClone.Entities
{
    public enum Privacy
    {
        Public,
        Private
    }

    public class List
    {
        
        public Int64 ID { get; set; }
        [Required]
        public string ListName { get; set; }
        public string Description { get; set; }
        public Privacy Privacy { get; set; }
        public User User { get; set; }
        public ICollection<User> Members { get; set; }
    }
}