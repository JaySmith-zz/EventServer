namespace EventServer.Core.Domain
{
    public class Category : Entity
    {
        public Category() { }

        public Category(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public string Title { get; set; }
        public string Description { get; set; }
    }
}