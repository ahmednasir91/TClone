using System;
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
        public bool IsFavourite { get; set; }
        public DateTime? DateAndTime { get; set; }

        public string ToJson(string username = "")
        {
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
                            IsFavorite = IsFavourite && String.Equals(Sender.Username, username, StringComparison.CurrentCultureIgnoreCase) ? "favorited" : "",
                        });
        }

    }
}