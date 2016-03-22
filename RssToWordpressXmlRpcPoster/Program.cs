using System;
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
            Console.WriteLine("Please wait for completion...");
            try
            {
                RssFeedToWP rssToWp = new RssFeedToWP(path);

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
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong, message: " + e +" Try again...");
                Console.ReadLine();
            }
        }
    }
}
