namespace FirstWebsite.Models
{
    public class Project
    {
        public string Id { get; set; } // Add this!
        public string Title { get; set; }
        public string Tool { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string Challenge { get; set; } // Added from your PDF
        public string Solution { get; set; }  // Added from your PDF
    }
}
