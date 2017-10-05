using CBIB.Models;
using CBIB.Views.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CBIB.Controllers
{
    public class HomeController : Controller
    {
        private readonly CBIBContext _CBIBContext;

        public HomeController(CBIBContext CBIBContext)
        {
            _CBIBContext = CBIBContext;
        }
        public IActionResult Index()
        {
            IDictionary<Journal, Author> dict = new Dictionary<Journal, Author>();

            List<Author> authors = _CBIBContext.Author.ToList();
            List<Journal> journals = _CBIBContext.Journal.ToList();

            dict = Display(journals, authors);

            var vm = new HomeIndexViewModel
            {
                dict = dict
            };

            return View(vm);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        private IDictionary<Journal, Author> Display(List<Journal> journalList, List<Author> authorList)
        {
            IDictionary<Journal, Author> dictionary = new Dictionary<Journal, Author>();

            foreach (Journal j in journalList)
            {
                foreach (Author a in authorList)
                {
                    if (j.AuthorID.Equals(a.AuthorID))
                    {
                        dictionary.Add(j, a);
                    }
                }
            }
            return dictionary;
        }
    }
}
