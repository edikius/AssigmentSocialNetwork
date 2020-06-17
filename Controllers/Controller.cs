﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SocialNetworkHomeAssigment.Controllers
{
    abstract public class AbstractController:Controller
    {
        protected RestClient.RestClient restClient;

        public AbstractController()
        {
            restClient = new RestClient.RestClient();
        }
    }
}
