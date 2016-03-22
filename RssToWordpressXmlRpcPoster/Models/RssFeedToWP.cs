using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using RssToWordpressXmlRpcPoster.Models.RssUptime;
using RssToWordpressXmlRpcPoster.Services;

namespace RssToWordpressXmlRpcPoster.Models
{
    public class RssFeedToWP
    {

        private ReadabilityService readerService;
        private RssFeedService rssFeed;
        public WordPress wpClient;

        public RssFeedToWP(string path, string feed)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = Directory.GetCurrentDirectory() + "/Keys.xml";
            }
            Initialize(path);
            readerService = new ReadabilityService(path);
            rssFeed = new RssFeedService(feed);
        }

        private void Initialize(string path)
        {
            var x = new XmlDocument();
            x.Load(path);
            string username = x.SelectSingleNode("//WordPress/Username").InnerText;
            string password = x.SelectSingleNode("//WordPress/Password").InnerText;
            string site = x.SelectSingleNode("//WordPress/Site").InnerText;
            //string site = x.SelectSingleNode("//Feed/Url").InnerText;
            wpClient = new WordPress(username, password, site);
        }

        public List<RssWithUrl> PostsFromReadability(List<RssModel> posts)
        {
            //var items = feedService.GetRssFeed();
            var data = new List<RssWithUrl>();
            foreach (var item in posts)
            {
                var rssWithUrl = new RssWithUrl(item, readerService.GetParsingUrl(item.Link));
                rssWithUrl.ParsedJson = readerService.GetParsedJson(rssWithUrl.RequestUrl);
                data.Add(rssWithUrl);
            }
            return data;
        }

        public List<RssWithUrl> GetNonDuplicatePosts()
        {

            var posts = wpClient.GetPosts();          
            var feeditems = PostsFromReadability(rssFeed.GetRssFeed());

            //var latestPost = feeditems.OrderByDescending(item => DateTime.Parse(item.RssModel.PubDate)).First();

            //if ()
            //{
                
            //}

            List<RssWithUrl> postsToAdd = new List<RssWithUrl>();
            for (int i = feeditems.Count - 1; i >= 0; i--)
            {
                var x = posts.FirstOrDefault(post => post.Title.Equals(feeditems.ElementAt(i).ParsedJson.Title));
                if (x == null)
                {
                    //Console.WriteLine(feeditems.ElementAt(i).ParsedJson.Title + " is from reader VS " + posts.ElementAt(i));
                    postsToAdd.Add(feeditems.ElementAt(i));
                    //feeditems.Remove(feeditems.ElementAt(i));
                }
            }
            return postsToAdd;
        }


    }
}
