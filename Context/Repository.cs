using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwitterClone.Entities;
using TwitterClone.Helpers;

namespace TwitterClone.Context
{
    public class Repository
    {
        public DataContext datacontext;
        public Repository()
        {
            datacontext = new DataContext();
        }

        public User GetUser(string username)
        {
            return datacontext.Users.SingleOrDefault(u => String.Equals(u.Username, username));
        }

        public User GetCurrentUser()
        {
            return GetUser(HttpContext.Current.User.Identity.Name);
        }

        public void NewTweet(Tweet tweet)
        {
            datacontext.Tweets.Add(tweet);
            datacontext.SaveChanges();
        }
        public List<Tweet> GetTweets(string username)
        {
            return datacontext.Tweets.Include("Sender").Where(t => String.Equals(t.Sender.Username, username)).ToList();
        }

        public List<Tweet> GetCurentUserTweets(bool includeFollowers = false)
        {
            var tweets = GetTweets(HttpContext.Current.User.Identity.Name);
            if (includeFollowers)
                foreach (var followingUser in GetCurrentUser().Following)
                    tweets.AddRange(followingUser.Tweets);
            return tweets.OrderBy(t => t.DateAndTime).ToList();
        }

        public Tweet GetTweet(int id)
        {
            return datacontext.Tweets.SingleOrDefault(t => t.Id == id);
        }
        public void DeleteTweet(int id)
        {
            Tweet tw =datacontext.Tweets.SingleOrDefault(t => t.Id == id);
            datacontext.Tweets.Remove(tw);
            datacontext.SaveChanges();
        }
        public bool Favorite(int id, bool what = true)
        {
            GetTweet(id).IsFavourite = what;
            return datacontext.SaveChanges() > 0;
        }

        public List<User> FollowerSuggestion()
        {
            var currentFollowers = GetCurrentUser().Following.AsEnumerable();
            var remaining = datacontext.Users.AsEnumerable();
            remaining = remaining.Except(currentFollowers);
            return
                remaining.Where(
                    u =>
                    !String.Equals(u.Username, HttpContext.Current.User.Identity.Name, StringComparison.CurrentCultureIgnoreCase))
                           .OrderBy(u => Guid.NewGuid())
                           .Take(3).ToList();
        }

        public bool Follower(string username)
        {
            GetCurrentUser().Following.Add(GetUser(username));
            return datacontext.SaveChanges() > 0;
        }
    }
}