using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using EventServer.Core.Domain;

namespace EventServer.Core.ViewModels
{
    public class SessionsIndexModel
    {
        public string ContentAction { get; set; }
        public string ListLinkCssClass { get; set; }
        public string TracksLinkCssClass { get; set; }
        public string TimesLinkCssClass { get; set; }
    }

    public class SessionsAddEditModel
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Title")]
        public string Title { get; set; }

        [Required]
        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Presentation Level")]
        public PresentationLevel Level { get; set; }

        [DisplayName("Presentation Category")]
        public PresentationCategory Category { get; set; }

        [DisplayName("Track")]
        public string Track { get; set; }

        [DisplayName("Time Slot")]
        public string TimeSlot { get; set; }
    }

    public class SessionsAdminBoxModel
    {
        public int SessionId { get; set; }
        public bool CanEdit { get; set; }
        public bool CanAcceptReject { get; set; }
        public UserProfile User { get; set; }
        public Comment[] Comments { get; set; }
    }

    public class SessionsGetByTrackModel
    {
        public SessionsGetByTrackModel(IEnumerable<Presentation> presentations, bool isAdmin)
        {
            Presentations = presentations.ToArray();
            IsAdmin = isAdmin;
        }

        public Presentation[] Presentations { get; set; }
        public bool IsAdmin { get; set; }
    }
}