using System;
using RssToWordpressXmlRpcPoster.Models.RssUptime;
using WordPressSharp;
using WordPressSharp.Models;

namespace RssToWordpressXmlRpcPoster.Models
{
    public class WordPress
    {
        private WordPressSiteConfig config;

        public WordPress(string username, string password, string site)
        {
            config = new WordPressSiteConfig
            {
                // http://localhost:8081/wp
                BaseUrl = site,
                BlogId = 1,
                Username = username,
                Password = password
            };
        }

        public Post[] GetPosts()
        {
            //var id;
            Post[] posts;
            using (var client = new WordPressClient(config))
            {
                PostFilter filter = new PostFilter();
                filter.PostType = "post";
                filter.PostStatus = "publish";
                posts = client.GetPosts(filter);               
            }
            return posts;
        }
        public void MakeNewPost(RssWithUrl postData)
        {
            // Author null?
            var newPost = new Post
            {
                Title = postData.ParsedJson.Title,
                PublishDateTime = (postData.ParsedJson.Date_published != null) ? DateTime.Parse(postData.ParsedJson.Date_published) : DateTime.Parse(postData.RssModel.PubDate),
                Content = postData.ParsedJson.Content + "<p>" + "<a href =\"" + postData.RssModel.Link + "\">" + "To original site.." + "</a></p>",
                PostType = "post",
                Status = "publish"
            };
            //var id;
            using (var client = new WordPressClient(config))
            {
                client.NewPost(newPost);
            }
            //return id;
        }
    }
}
