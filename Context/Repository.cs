using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
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
            return datacontext.Users.Include("Tweets").SingleOrDefault(u => String.Equals(u.Username, username));
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
                    tweets.AddRange(GetTweets(followingUser.Username));
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
            if (what)
                GetCurrentUser().Favourites.Add(GetTweet(id));
            else
                GetCurrentUser().Favourites.Remove(GetTweet(id));
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

        public List<Tweet> GetFavourites(bool withSender = false, string username = "")
        {
            if (withSender)
            {
                var favs = GetCurrentUser().Favourites.Select(f => f.Id);
                return
                    datacontext.Tweets.Include("Sender")
                               .Where(u => favs.Any(f => f == u.Id))
                               .ToList();
            }
            return String.IsNullOrEmpty(username) ? GetCurrentUser().Favourites.ToList() : GetUser(username).Favourites.ToList();
        }

        public void NewList(List list)
        {
            list.User = GetCurrentUser();
            GetCurrentUser().Lists.Add(list);
            datacontext.SaveChanges();
        }

        public List<List> Lists(string username)
        {
            return
                datacontext.Lists.Include("User")
                           .Where(u => String.Equals(u.User.Username, username)).ToList();
        }

        public void UpdateUserBasic(User user)
        {
            var us = GetCurrentUser();
            us.Username = user.Username;
            us.Email = user.Email;
            datacontext.SaveChanges();
        }

        public void UpdateUserInfo(User user)
        {
            var us = GetCurrentUser();
            us.FullName = user.FullName;
            us.WebsiteURL = user.WebsiteURL;
            us.Bio = user.Bio;
            us.Location = user.Location;
            datacontext.SaveChanges();
        }

        public void SaveAvatar(HttpPostedFileBase avatar)
        {
            var fileName = HttpContext.Current.User.Identity.Name + Path.GetExtension(avatar.FileName);
            var path = Path.Combine(HostingEnvironment.MapPath("~/Content/Images/"), fileName);
            avatar.SaveAs(path);
            GetCurrentUser().Avatar = fileName;
            datacontext.SaveChanges();
        }

        public string SaveBackground(HttpPostedFileBase background)
        {
            var fileName = HttpContext.Current.User.Identity.Name + Path.GetExtension(background.FileName);
            var path = Path.Combine(HostingEnvironment.MapPath("~/Content/themes/bgs/"), fileName);
            background.SaveAs(path);
            GetCurrentUser().BackgroundImage = fileName;
            return "/Content/themes/bgs/" + fileName;
        }

        public bool UpdateDesign(User user)
        {
            var us = GetCurrentUser();
            us.BackgroundImage = user.BackgroundImage.Substring(user.BackgroundImage.LastIndexOf('/') + 1);
            us.Tiled = user.Tiled;
            us.LinkColor = user.LinkColor;
            us.BackgroundColor = user.BackgroundColor;
            return datacontext.SaveChanges() > 0;
        }
    }
}