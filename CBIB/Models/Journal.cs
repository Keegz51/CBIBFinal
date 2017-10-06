using System.ComponentModel.DataAnnotations;

namespace CBIB.Models
{
    public class Journal
    {
        public long ID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Research Output")]
        public string Type { get; set; }

        [Display(Name = "Author 1")]
        public string CoAuthor1 { get; set; }

        [Display(Name = "Author 2")]
        public string CoAuthor2 { get; set; }

        [Required]
        public string Year { get; set; }

        [Required]
        public string Abstract { get; set; }

        [Display(Name = "Proof of Peer Review")]
        public string ProofOfpeerReview { get; set; }

        [Display(Name = "Peer Reviewed?")]

        public bool PeerReviewed { get; set; }

        public string url { get; set; }

        public string PeerUrl { get; set; }

        public long AuthorID { get; set; }

        public Author Author { get; set; }
    }
}
