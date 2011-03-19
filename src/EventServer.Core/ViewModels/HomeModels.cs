using System;
using EventServer.Core.Domain;

namespace EventServer.Core.ViewModels
{
    public class HomeIndexModel
    {
        public Post[] Posts { get; set; }
        public Tweet[] Tweets { get; set; }
        public Sponsor[] Sponsors { get; set; }
    }
}