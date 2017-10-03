using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CBIB.Models
{
    public class Node
    {
        public long ID { get; set; }
        [Display(Name = "Node")]
        public string Name { get; set; }

        //list of authors on the node
        public ICollection<Author> Authors { get; set; }
    }
}
