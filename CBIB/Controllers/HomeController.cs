using CBIB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace CBIB.Controllers
{
    public class HomeController : Controller
    {
        private readonly CBIBContext _CBIBContext;

        public HomeController(CBIBContext CBIBContext)
        {
            _CBIBContext = CBIBContext;
        }
        public async Task<IActionResult> Index(string authorString, string type, string year, bool reviewed, string node)
        {
            List<Journal> q = await _CBIBContext.Journal.Include(a => a.Author).ToListAsync();

            if ((!String.IsNullOrEmpty(authorString)))
            {
                q = q.Where(p => p.Author.Name.ToLower().Contains(authorString.ToLower().Trim())).ToList();
            }
            if ((!String.IsNullOrEmpty(type)))
            {
                q = q.Where(p => p.Type.ToLower().Contains(type.ToLower().Trim())).ToList();
            }
            if ((!String.IsNullOrEmpty(year)))
            {
                q = q.Where(p => p.Year.ToLower().Contains(year.ToLower().Trim())).ToList();
            }
            //if ((!String.IsNullOrEmpty(node)))
            //{
            //    q = q.Where(p=>p.Author.AuthorNode.Name.ToLower().Contains(node.Trim().ToLower())).ToList();
            //}

            return View(q);
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
