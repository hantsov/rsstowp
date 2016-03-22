using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using RssToWordpressXmlRpcPoster.Models.RssUptime;
using RssToWordpressXmlRpcPoster.Services;

namespace RssToWordpressXmlRpcPoster.Models
{
    /// <summary>
    /// Centralizes functionality of getting data from
    /// Readability, your RSS-feed and reading/writing to
    /// Wordpress
    /// </summary>
    public class RssFeedToWP
    {

        private ReadabilityService readerService;
        private RssFeedService rssFeed;
        public WordPress wpClient;

        public RssFeedToWP(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = Directory.GetCurrentDirectory() + "/Keys.xml";
            }
            if (File.Exists(path))
            {
                Initialize(path);
            }
            else
            {
                throw new ArgumentException("Path invalid or file not found at default location");
            }
        }

        private void Initialize(string path)
        {
            var x = new XmlDocument();
            x.Load(path);
            string username = x.SelectSingleNode("//WordPress/Username").InnerText;
            string password = x.SelectSingleNode("//WordPress/Password").InnerText;
            string site = x.SelectSingleNode("//WordPress/Site").InnerText;
            string feedUrl = x.SelectSingleNode("//Feed/Url").InnerText;
            readerService = new ReadabilityService(path);
            rssFeed = new RssFeedService(feedUrl);
            wpClient = new WordPress(username, password, site);
        }

        public List<RssWithUrl> PostsFromReadability(List<RssModel> posts)
        {
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
            var feeditems = rssFeed.GetRssFeed();

            var latestPost = posts.OrderByDescending(item => item.PublishDateTime).FirstOrDefault();
            if (latestPost != null)
            {
                feeditems =
                    feeditems.Where(feeditem => DateTime.Parse(feeditem.PubDate) > latestPost.PublishDateTime)
                        .ToList();
            }
            var postsToAdd = PostsFromReadability(feeditems);
            // if updating fails in the middle, add posts from earliest to latest
            postsToAdd = postsToAdd.OrderBy(post => DateTime.Parse(post.RssModel.PubDate)).ToList();
            return postsToAdd;
        }


    }
}
