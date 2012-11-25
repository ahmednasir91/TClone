using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;

namespace TwitterClone.Entities
{
    public class User
    {
        [Key]
        public virtual string Username { get; set; }

        [Required]
        public virtual string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public virtual string Password { get; set; }
        [Required]
        public virtual string FullName { get; set; }

        [DataType(DataType.MultilineText)]
        public virtual string Comment { get; set; }

        public virtual string Bio { get; set; }
        public virtual string Location { get; set; }
        public virtual string Website { get; set; }
        public virtual string WebsiteURL { get; set; }
        public virtual ICollection<List> Lists { get; set; } 
        public virtual ICollection<Tweet> Favourites { get; set; }

        public virtual string BackgroundImage { get; set; }
        public virtual string BackgroundColor { get; set; }
        public virtual string LinkColor { get; set; }
        public virtual Boolean Tiled { get; set; }

        
        public virtual ICollection<Role> Roles { get; set; }
        [ScriptIgnore]
        public virtual ICollection<Tweet> Tweets { get; set; }

        public virtual ICollection<User> Followers { get; set; }

        public virtual ICollection<User> Following { get; set; }
        public string Avatar { get; set; }

        public string GetBackgroundImage()
        {
            return "/Content/themes/bgs/" + (String.IsNullOrEmpty(BackgroundImage) ? "theme1.png" : BackgroundImage);
        }
        public string GetAvatar(string size)
        {
            switch(size)
            {
                case "small":
                    return "/Content/Images/" + (Avatar ?? "DefaultAvatar.png");
            }
            return "";
        }
        public string Count(string type)
        {
            var count = 0;
            switch (type)
            {
                case "Tweets":
                    count = Tweets.Count;
                    break;
                case "Followers":
                    count = Followers.Count;
                    break;
                case "Following":
                    count = Following.Count;
                    break;
            }
            return count.ToString();
        }
    }
}