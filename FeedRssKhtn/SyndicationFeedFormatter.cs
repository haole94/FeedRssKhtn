using FeedRssKhtn.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace FeedRssKhtn
{
    public class SyndicationFeedFormatter: MediaTypeFormatter
    {
        private readonly string rss = "application/rss+xml";

        public SyndicationFeedFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(rss));
        }

        public override bool CanWriteType(Type type)
        {
            if (type == typeof(IEnumerable<Url>))
                return true;

            return false;
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext)
        {
            return Task.Factory.StartNew(() =>
            {
                if (type == typeof(IEnumerable<Url>))
                    BuildSyndicationFeed(value, writeStream);
            });
        }

        private void BuildSyndicationFeed(object models, Stream stream)
        {
            List<SyndicationItem> items = new List<SyndicationItem>();
            var feed = new SyndicationFeed()
            {
                Title = new TextSyndicationContent("My Feed")
            };

            if (models is IEnumerable<Url>)
            {
                var enumerator = ((IEnumerable<Url>)models).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    items.Add(BuildSyndicationItem(enumerator.Current));
                }
            }
            else
            {
                items.Add(BuildSyndicationItem((Url)models));
            }

            feed.Items = items;

            using (XmlWriter writer = XmlWriter.Create(stream))
            {
                Rss20FeedFormatter rssformatter = new Rss20FeedFormatter(feed);
                rssformatter.WriteTo(writer);
            }
        }

        private SyndicationItem BuildSyndicationItem(Url u)
        {
            var item = new SyndicationItem()
            {
                Title = new TextSyndicationContent(u.Title),
                BaseUri = new Uri("http://www.fit.hcmus.edu.vn/vn/"),
                LastUpdatedTime = new DateTimeOffset(new DateTime(u.Year, u.Month, u.Day)),
                Content = new TextSyndicationContent(u.Title)
            };
            item.Authors.Add(new SyndicationPerson() { Name = ""});
            return item;
        }
    }
}