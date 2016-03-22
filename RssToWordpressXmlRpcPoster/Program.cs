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
            
            var rssToWp = new RssFeedToWP();

            var newPosts = rssToWp.GetNonDuplicatePosts();

            foreach (var post in newPosts)
            {
                rssToWp.wpClient.MakeNewPost(post);
            }
        }
    }
}
