using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

using SocialNetworkHomeAssigment.Endpoint;
using SocialNetworkHomeAssigment.Models;
using SocialNetworkHomeAssigment.Parser;
using SocialNetworkHomeAssigment.RestClientTwitter;

namespace SocialNetworkHomeAssigment.Controllers
{
    public class TwitterController : AbstractController
    {
        private const string consumer_key = "N2Jn3wf4aRmtTdhqBQL3S7EQp";
        private const string consumer_secret = "0prkX7BmgJVPwOR80cMBOqMYb6qxtAAr4moWSgZfEiOFqPt2oj";
        private const string access_token = "1269631622883815426-evbHY8VUyU4qelluy9UlhVvJ7Su6JQ";
        private const string token_secret = "6ynEtujTm27rbvPNn8gEQz1DSfX4CM9eQIovCPBS5QHyA";
        private twitterEndpoint twEndpoint;
        public const string OauthSignatureMethod = "HMAC-SHA1";
        public const string OauthVersion = "1.0";
        public string AuthSignature = "";

        protected RestClientTwitter.RestClientTwitter restClient = new RestClientTwitter.RestClientTwitter();

        private string createHeader(string resourceURL, Method method, string msg = "")
        {
            var authNonce = CreateOauthNonce();
            var authTime = CreateOAuthTimestamp();
            var sig = CreateOauthSignature(resourceURL, method, authNonce, authTime, msg);

            StringBuilder b = new StringBuilder();
            b.Append($"OAuth oauth_consumer_key={consumer_key},");
            b.Append($"oauth_nonce={authNonce},");
            b.Append($"oauth_signature_method={OauthSignatureMethod},");
            b.Append($"oauth_timestamp={authTime},");
            b.Append($"oauth_token={access_token},");
            b.Append($"oauth_version={OauthVersion},");
            b.Append($"oauth_signature={sig}");

            var sigBaseString = b.ToString();

            return sigBaseString;

        }

        private string CreateOauthSignature(string resourceUrl, Method method, string oauthNonce, string oauthTimestamp, string msg)
        {
            var baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                            "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}";

            if (msg != "")
            {
                baseFormat += "&status=" + Uri.EscapeDataString(msg);
            }

            var baseString = string.Format(baseFormat,
                                        consumer_key,
                                        oauthNonce,
                                        OauthSignatureMethod,
                                        oauthTimestamp,
                                        access_token,
                                        OauthVersion
                                        );

            baseString = string.Concat(method + "&", Uri.EscapeDataString(resourceUrl), "&", Uri.EscapeDataString(baseString));



            var compositeKey = string.Concat(Uri.EscapeDataString(consumer_secret),
                                    "&", Uri.EscapeDataString(token_secret));

            string oauth_signature;
            using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
            {
                oauth_signature = Convert.ToBase64String(
                    hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            }

            string autht = Uri.EscapeDataString(oauth_signature);
            return autht;

        }

        private static string CreateOAuthTimestamp()
        {

            var nowUtc = DateTime.UtcNow;
            var timeSpan = nowUtc - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

            return timestamp;
        }
        public enum Method
        {
            POST,
            GET
        }

        private string CreateOauthNonce()
        {
            var oauthNonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture)));
            return oauthNonce;
        }
        public TwitterController() : base()
        {
            twEndpoint = new twitterEndpoint();
        }

        public JsonResult getTimeline()
        {
           
            AuthSignature = createHeader(twEndpoint.getTimeline(), Method.GET);
            twEndpoint.setAuth(AuthSignature);
            restClient.endpoint = twEndpoint.getTimeline();
            string response = restClient.makeRequest(RestClientTwitter.HttpVerb.GET, twEndpoint.getHeader());  
            List<TwitterPost> dataList = new List<TwitterPost>();

            

            TwitterFeedModel deserializedfbModel = new TwitterFeedModel();
            string addeddata = "{\"data\": ";
            string addeddata2 = "}";
            response = addeddata + response + addeddata2;

            deserializedfbModel = JsonConvert.DeserializeObject<TwitterFeedModel>(response);

            foreach (TwitterPost t in deserializedfbModel.data)
            {
                dataList.Add(t);
            }

            return Json(new { success = true, dataList = dataList }, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult getFriends()
        {

            AuthSignature = createHeader(twEndpoint.getFriends(), Method.GET);
            twEndpoint.setAuth(AuthSignature);
            restClient.endpoint = twEndpoint.getFriends();
            string response = restClient.makeRequest(RestClientTwitter.HttpVerb.GET, twEndpoint.getHeader());
            List<twitterFriend> dataList = new List<twitterFriend>();



            TwitterFriendModel deserializedfbModel = new TwitterFriendModel();


            deserializedfbModel = JsonConvert.DeserializeObject<TwitterFriendModel>(response);

            foreach (twitterFriend t in deserializedfbModel.users)
            {
                dataList.Add(t);
            }

            return Json(new { success = true, dataList = dataList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getFollowers()
        {

            AuthSignature = createHeader(twEndpoint.getFollower(), Method.GET);
            twEndpoint.setAuth(AuthSignature);
            restClient.endpoint = twEndpoint.getFollower();
            string response = restClient.makeRequest(RestClientTwitter.HttpVerb.GET, twEndpoint.getHeader());
            List<twitterFriend> dataList = new List<twitterFriend>();



            TwitterFriendModel deserializedfbModel = new TwitterFriendModel();


            deserializedfbModel = JsonConvert.DeserializeObject<TwitterFriendModel>(response);

            foreach (twitterFriend t in deserializedfbModel.users)
            {
                dataList.Add(t);
            }

            return Json(new { success = true, dataList = dataList }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult getfavourites()
        {

            AuthSignature = createHeader(twEndpoint.getFavourites(), Method.GET);
            twEndpoint.setAuth(AuthSignature);
            restClient.endpoint = twEndpoint.getFavourites();
            string response = restClient.makeRequest(RestClientTwitter.HttpVerb.GET, twEndpoint.getHeader());
            List<TwitterPost> dataList = new List<TwitterPost>();



            TwitterFeedModel deserializedfbModel = new TwitterFeedModel();
            string addeddata = "{\"data\": ";
            string addeddata2 = "}";
            response = addeddata + response + addeddata2;

            deserializedfbModel = JsonConvert.DeserializeObject<TwitterFeedModel>(response);

            foreach (TwitterPost t in deserializedfbModel.data)
            {
                dataList.Add(t);
            }

            return Json(new { success = true, dataList = dataList }, JsonRequestBehavior.AllowGet);
        }

    }

}