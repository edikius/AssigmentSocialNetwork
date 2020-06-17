using SocialNetworkHomeAssigment.Endpoint;
using SocialNetworkHomeAssigment.Models;
using SocialNetworkHomeAssigment;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using SocialNetworkHomeAssigment.RestClient;

namespace SocialNetworkHomeAssigment.Controllers
{
    
    public class PagePostsController : AbstractController
    {

        private facebookEndpoint facebookEndpoint;

        public PagePostsController() : base()
        { 
            facebookEndpoint = new facebookEndpoint();
        }

        public JsonResult getPosts(string access_token)
        {
            
            
            List<Data> dataList = new List<Data>();
            restClient.endpoint = facebookEndpoint.getPosts(access_token);
            string response = restClient.makeRequest(RestClient.HttpVerb.GET);
            PagePostsModel deserializedfbModel = new PagePostsModel();
            deserializedfbModel = JsonConvert.DeserializeObject<PagePostsModel>(response);
            foreach (Data d in deserializedfbModel.data)
            {
                dataList.Add(d);
            }
            return Json(new { success = true, dataList = dataList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getLikes(string access_token)
        {
            System.Diagnostics.Debug.WriteLine(access_token);
            List<Data1> dataList = new List<Data1>();
            restClient.endpoint = facebookEndpoint.getLikes(access_token);

            string response = restClient.makeRequest(RestClient.HttpVerb.GET);
            PageLikeModels deserializedfbModel = new PageLikeModels();
            
            deserializedfbModel = JsonConvert.DeserializeObject<PageLikeModels>(response);
            foreach (Data1 d in deserializedfbModel.data)
            {
                dataList.Add(d);
            }
            return Json(new { success = true, dataList = dataList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getPageAccessPoint(string accessToken)
        {
            System.Diagnostics.Debug.WriteLine(accessToken);
            List<DataPageToken> dataList = new List<DataPageToken>();
            
            restClient.endpoint = facebookEndpoint.getPageAccessPoint(accessToken);
            
            string response = restClient.makeRequest(RestClient.HttpVerb.GET);
            
            PageTokenModel deserializedfbModel = new PageTokenModel();
            deserializedfbModel = JsonConvert.DeserializeObject<PageTokenModel>(response);
            foreach (DataPageToken d in deserializedfbModel.data)
            {
                dataList.Add(d);
            }
            return Json(new { success = true, dataList = dataList }, JsonRequestBehavior.AllowGet);
        }

        
    }
}
