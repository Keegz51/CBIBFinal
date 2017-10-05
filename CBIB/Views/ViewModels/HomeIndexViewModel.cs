using CBIB.Models;
using System.Collections.Generic;

namespace CBIB.Views.ViewModels
{
    public class HomeIndexViewModel
    {
        public IDictionary<Journal, Author> dict = new Dictionary<Journal, Author>();
    }
}
