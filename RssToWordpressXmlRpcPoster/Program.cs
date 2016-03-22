using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RssToWordpressXmlRpcPoster.Models;

namespace RssToWordpressXmlRpcPoster
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Please construct a Keys.xml file with specified structure " +
                              "and type the path to that file here (leave empty if using default path):");
            var path = Console.ReadLine();
            Console.WriteLine("Please enter a rss feed to pull from (leave empty if default)_:");
            var feed = Console.ReadLine();
            Console.WriteLine("Please wait for completion...");
            RssFeedToWP rssToWp = new RssFeedToWP(path, feed);

            var newPosts = rssToWp.GetNonDuplicatePosts();
            if (newPosts != null && newPosts.Count > 0)
            {
                Console.WriteLine("Found " + newPosts.Count + " new posts to publish. Publishing...");
                foreach (var post in newPosts)
                {
                    rssToWp.wpClient.MakeNewPost(post);
                }
            }
        }
    }
}
