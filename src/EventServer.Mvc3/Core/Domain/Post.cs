using System;
using System.Collections.Generic;
using System.Linq;

namespace EventServer.Core.Domain
{
    public class Post : Entity
    {
        public Post()
        {
        }

        public Post(string author, string title, string content, string description, string slug, bool isPublished)
        {
            Author = author;
            Title = title;
            Content = content;
            Description = description;
            Slug = slug;
            IsPublished = isPublished;

            Categories = new List<Category>();
            Tags = new List<string>();

            DateCreated = DateTime.Now;
        }

        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public List<Category> Categories { get; set; }
        public List<string> Tags { get; set; }
        public bool IsPublished { get; set; }
        public string Slug { get; set; }
        public DateTime DateCreated { get; set; }
        public string Keywords { get; set; }
    }

    public static class PostExtensions
    {
        public static Post[] Published(this IQueryable<Post> source)
        {
            return source
                .Where(x => x.IsPublished)
                .Where(x => x.DateCreated <= DateTime.Now)
                .OrderByDescending(x => x.DateCreated)
                .ToArray();
        }
    }
}