using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR.Hubs;
using Newtonsoft.Json.Linq;
using TwitterClone.Context;

namespace TwitterClone.Hubs
{
    [HubName("followerhub")]
    public class FollowerHub : Hub
    {
        readonly Repository repository = new Repository();

        public void GetSuggestions()
        {
            JArray suggestions = new JArray();
            foreach (var suggestion in repository.FollowerSuggestion())
                suggestions.Add(new JObject
                                    {
                                        new JProperty("UserName", suggestion.Username),
                                        new JProperty("Avatar", suggestion.Avatar ?? "/Content/Images/DefaultAvatar.png"),
                                        new JProperty("FullName", suggestion.FullName),
                                        new JProperty("Url", "/" + suggestion.Username),
                                    });
            Caller.addSuggestions(suggestions);
        } 

        public bool AddFollower(string username)
        {
            return repository.Follower(username);
        }
    }
}
