using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;

namespace TwitterClone.Entities
{
    public class Tweet
    {
        [Key]
        public long Id { get; set; }    
        [Required]
        public string Message { get; set; }
        public User Sender { get; set; }
        public Guid SenderId { get; set; }
        public DateTime? DateAndTime { get; set; }

        public string ToJson(string username = "", List<Tweet> favorites = null)
        {
            favorites = favorites ?? new List<Tweet>();
            var serializer = new JavaScriptSerializer();
            return
                serializer.Serialize(
                    new
                        {
                            Id = Id,
                            Message = Message,
                            SenderName = Sender.FullName,
                            SenderUserName = Sender.Username,
                            DateAndTime = ((DateAndTime.HasValue ? DateAndTime.Value : DateTime.Now) - new DateTime(1970, 1, 1)).TotalMilliseconds,
                            Url = "/" + Sender.Username + "/status/" + Id,
                            UserProfileUrl = "/" + Sender.Username,
                            IsFavorite = (favorites.Any(t => t.Id == Id)) ? "favorited" : "",
                            Avatar = Sender.GetAvatar("small"),
                            IsMine = String.Equals(Sender.Username, username, StringComparison.CurrentCultureIgnoreCase),
                        });
        }

    }
}