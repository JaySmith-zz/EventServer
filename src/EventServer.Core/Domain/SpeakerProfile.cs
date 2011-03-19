using System;
using System.Collections.Generic;

namespace EventServer.Core.Domain
{
    public class SpeakerProfile
    {
        public string Biography { get; set; }
        public string ImageUrl { get; set; }
        public string BlogUrl { get; set; }
        public bool IsMvp { get; set; }
        public string MvpProfileUrl { get; set; }
        public bool TravelAssistance { get; set; }
        public List<SpeakerAccolade> Accolades { get; set; }

        public bool HasBlogUrl
        {
            get { return !String.IsNullOrEmpty(BlogUrl); }
        }

        public bool HasImageUrl
        {
            get { return !String.IsNullOrEmpty(ImageUrl); }
        }
    }
}