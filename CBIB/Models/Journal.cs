using System.ComponentModel.DataAnnotations;

namespace CBIB.Models
{
    public class Journal
    {
        public long ID { get; set; }
        public string Title { get; set; }

        [Display(Name = "Research Output")]
        public string Type { get; set; }

        [Display(Name = "First Co-Author")]
        public string CoAuthor1 { get; set; }

        [Display(Name = "Second Co-Author")]
        public string CoAuthor2 { get; set; }

        public string Year { get; set; }

        public string Abstract { get; set; }

        [Display(Name = "Proof of Peer Review")]
        public string ProofOfpeerReview { get; set; }

        [Display(Name = "Peer Reviewed?")]

        public bool PeerReviewed { get; set; }

        public string url { get; set; }

        public string PeerUrl { get; set; }

        public long AuthorID { get; set; }
    }
}
