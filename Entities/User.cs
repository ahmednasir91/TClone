using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;

namespace TwitterClone.Entities
{
    public class User
    {
        [Key]
        public virtual Guid UserId { get; set; }

        [Required]
        public virtual String Username { get; set; }

        [Required]
        public virtual String Email { get; set; }

        [Required, DataType(DataType.Password)]
        public virtual String Password { get; set; }
        [Required]
        public virtual String FullName { get; set; }

        [DataType(DataType.MultilineText)]
        public virtual String Comment { get; set; }

        public virtual String Bio { get; set; }
        public virtual String Location { get; set; }
        public virtual String Website { get; set; }
        public virtual String WebsiteURL { get; set; }
        public virtual ICollection<List> Lists { get; set; } 
        public virtual ICollection<Tweet> Favourites { get; set; }

        public virtual String BackgroundImage { get; set; }
        public virtual String BackgroundColor { get; set; }
        public virtual String LinkColor { get; set; }
        public virtual Boolean Tiled { get; set; }

        public virtual Boolean IsApproved { get; set; }
        public virtual int PasswordFailuresSinceLastSuccess { get; set; }
        public virtual DateTime? LastPasswordFailureDate { get; set; }
        public virtual DateTime? LastActivityDate { get; set; }
        public virtual DateTime? LastLockoutDate { get; set; }
        public virtual DateTime? LastLoginDate { get; set; }
        public virtual String ConfirmationToken { get; set; }
        public virtual DateTime? CreateDate { get; set; }
        public virtual Boolean IsLockedOut { get; set; }
        public virtual DateTime? LastPasswordChangedDate { get; set; }
        public virtual String PasswordVerificationToken { get; set; }
        public virtual DateTime? PasswordVerificationTokenExpirationDate { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
        [ScriptIgnore]
        public virtual ICollection<Tweet> Tweets { get; set; }

        public virtual ICollection<User> Followers { get; set; }

        public virtual ICollection<User> Following { get; set; }
        public string Avatar { get; set; }

        public string GetBackgroundImage()
        {
            return "/Content/themes/bgs/" + BackgroundImage;
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