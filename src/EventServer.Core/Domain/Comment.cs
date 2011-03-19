using System;

namespace EventServer.Core.Domain
{
    public class Comment
    {
        public string Author { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
    }
}