using FeedRssKhtn.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Web.Http;

namespace FeedRssKhtn.Controllers
{
    public class ValuesController : ApiController
    {
        public IHttpActionResult Get()
        {
            FeedFunction func = new FeedFunction();
            List<Url> list = func.GetFeed("http://www.fit.hcmus.edu.vn/vn/");

            SyndicationFeed feed = new SyndicationFeed("My feed", "Test", new Uri("http://www.fit.hcmus.edu.vn/vn/"));
            List<SyndicationItem> items = new List<SyndicationItem>();

            foreach (Url u in list)
            {
                SyndicationItem item = new SyndicationItem(u.Title, u.Day + "/" + u.Month + "/" + u.Year, new Uri("http://www.fit.hcmus.edu.vn/vn/"));
                items.Add(item);
            }

            feed.Items = items;

            return Ok(new Rss20FeedFormatter(feed));
        }
    }
}
