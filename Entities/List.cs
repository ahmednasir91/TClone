using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterClone.Entities
{
    public class List
    {
        public Int64 ID { get; set; }
        public virtual Guid UserId { get; set; }
        public User MyUser { get; set; }
        public string ListName { get; set; }
    }
}