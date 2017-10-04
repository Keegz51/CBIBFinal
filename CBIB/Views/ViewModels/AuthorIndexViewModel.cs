using CBIB.Models;
using System.Collections.Generic;

namespace CBIB.Views.ViewModels
{
    public class AuthorIndexViewModel
    {
        public IDictionary<Author, string> dict = new Dictionary<Author, string>();
    }
}
