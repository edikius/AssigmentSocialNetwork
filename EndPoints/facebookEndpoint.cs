using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkHomeAssigment.Endpoint
{
    public class facebookEndpoint
    {
        private const string baseEndpoint = "https://graph.facebook.com";
        private const string version = "v7.0";
        public string getPosts(string accessToken)
        {
            StringBuilder stringBuilder = new StringBuilder(baseEndpoint);
            stringBuilder.Append($"/{version}");
            stringBuilder.Append("/me/posts?");
            stringBuilder.Append($"access_token={accessToken}");
            return stringBuilder.ToString();
        }
        public string getLikes(string accessToken)
        {
            StringBuilder stringBuilder = new StringBuilder(baseEndpoint);
            stringBuilder.Append($"/{version}");
            stringBuilder.Append("/me/likes?");
            stringBuilder.Append($"access_token={accessToken}");
            return stringBuilder.ToString();
        }
        
        public string getPageAccessPoint(string accessToken)
        {
            StringBuilder stringBuilder = new StringBuilder(baseEndpoint);
            stringBuilder.Append($"/{version}");
            stringBuilder.Append($"/me/accounts?fields=access_token");
            stringBuilder.Append($"&access_token={accessToken}");
            return stringBuilder.ToString();
        }
    }
}
