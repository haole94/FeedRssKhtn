using FeedRssKhtn.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace FeedRssKhtn
{
    public class FeedFunction
    {
        HtmlDocument doc;

        public HtmlDocument Doc
        {
            get { return doc; }
            set { doc = value; }
        }

        public List<Url> GetFeed(string link)
        {
            doc = new HtmlDocument();
            doc.LoadHtml(new WebClient().DownloadString(link));
            HtmlNode root = doc.DocumentNode;
            HtmlNodeCollection nodes = root.SelectNodes("//div[@id='dnn_ctr989_ModuleContent']/table");
            List<Url> result = new List<Url>();

            foreach (HtmlNode n in nodes)
            {
                HtmlNodeCollection col = n.SelectNodes(".//td");
                string day = col.ElementAt(0).InnerText;
                string year = col.ElementAt(1).InnerText;
                string title = col.ElementAt(2).SelectSingleNode(".//a").GetAttributeValue("title", "");
                string month = col.ElementAt(3).InnerText;
                Url url = new Url();
                url.Day = int.Parse(day);
                url.Month = int.Parse(month);
                url.Year = int.Parse(year);
                url.Title = title;
                result.Add(url);
            }

            return result;
        }
    }
}