namespace CBIB.Models
{
    public class Journal
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Abstract { get; set; }

        public string url { get; set; }

        public long AuthorID { get; set; }
    }
}
