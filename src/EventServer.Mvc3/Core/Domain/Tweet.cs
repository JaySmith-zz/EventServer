using System;

namespace EventServer.Core.Domain
{
    public class Tweet
    {
        public DateTime PostDate { get; set; }
        public string Message { get; set; }
    }
}