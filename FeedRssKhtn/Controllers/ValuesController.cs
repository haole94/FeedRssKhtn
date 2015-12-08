using FeedRssKhtn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FeedRssKhtn.Controllers
{
    public class ValuesController : ApiController
    {
        public IEnumerable<Url> Get()
        {
            FeedFunction func = new FeedFunction();
            IEnumerable<Url> result = func.GetFeed("http://www.fit.hcmus.edu.vn/vn/");

            return result;
        }
    }
}
