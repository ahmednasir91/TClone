﻿using System;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using SignalR.Hubs;
using TwitterClone.Context;
using TwitterClone.Entities;

namespace TwitterClone.Hubs
{
    [HubName("tweethub")]
    public class TweetHub : Hub
    {
        readonly Repository repository = new Repository();

        public void Connected()
        {
            Groups.Add(Context.ConnectionId, Context.User.Identity.Name);
            foreach (var following in repository.GetCurrentUser().Following)
            {
                Groups.Add(Context.ConnectionId, following.Username);
            }
        }

        public void Add(string message)
        {
            var tweet = new Tweet { Message = message, Sender = repository.GetCurrentUser(), DateAndTime = DateTime.Now };
            repository.NewTweet(tweet);
            Clients[Context.User.Identity.Name].ShowTweet(tweet.ToJson());
        }

        public void GetAll(string username = "", bool onlyFavs = false)
        {
            if (onlyFavs)
            {
                foreach (var favourite in repository.GetFavourites(true))
                    Caller.AddAll(favourite.ToJson(HttpContext.Current.User.Identity.Name, repository.GetFavourites()));
                return;
            }
            if (!String.IsNullOrEmpty(username))
            {
                foreach(var tweet in repository.GetTweets(username))
                    Caller.AddAll(tweet.ToJson(username, repository.GetFavourites(false, username)));
                return;
            }
            foreach (var tweet in repository.GetCurentUserTweets(String.IsNullOrEmpty(username)))
            {
                Caller.AddAll(tweet.ToJson(HttpContext.Current.User.Identity.Name, repository.GetFavourites()));
            }
        }

        public void Favorite(int id)
        {
            repository.Favorite(id);
        }

        public void UnFavorite(int id)
        {
            repository.Favorite(id, false);
        }
        public void Deletetweet(int id)
        {
            repository.DeleteTweet(id);
        }
    }
}
