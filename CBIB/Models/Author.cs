using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBIB.Models
{
    public class Author
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long AuthorID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        public ICollection<Journal> Journals { get; set; }
        public long NodeID { get; set; }

    }
}
