namespace BookstoreProject.Models
{
    public class Book
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Publisher { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public List<string> Content { get; set; }
        public string Url { get; set; }
        public int YearPublish { get; set; }
    }
}
