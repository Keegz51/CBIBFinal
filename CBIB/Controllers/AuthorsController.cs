using CBIB.Models;
using CBIB.Views.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBIB.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly CBIBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthorsController(CBIBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Authors
        public async Task<IActionResult> Index(string id)
        {
            //    var movies = from m in _context.Author select m;

            //if (!String.IsNullOrEmpty(id))
            //{
            //    movies = movies.Where(s => s.Name.Contains(id));
            //}
            //return View(await movies.ToListAsync());

            AuthorIndexViewModel avm = new AuthorIndexViewModel();
            List<Author> AuthorList = null;
            List<Node> NodesList = null;

            var user = await _userManager.GetUserAsync(User);
            Author currentAuthor = await _context.Author.FindAsync(user.AuthorID);

            if (User.IsInRole("Global Administrator"))
            {
                AuthorList = await _context.Author.ToListAsync();
                NodesList = await _context.Node.ToListAsync();
                avm.dict = Display(AuthorList, NodesList);
            }

            else if (User.IsInRole("Node Administrator"))
            {
                AuthorList = await _context.Author.Where(s => s.NodeID.Equals(currentAuthor.NodeID)).ToListAsync();
                NodesList = NodesList = await _context.Node.Where(s => s.ID.Equals(currentAuthor.NodeID)).ToListAsync();
                avm.dict = Display(AuthorList, NodesList);
            }
            else
            {
                var Authors = _context.Author.Where(s => s.NodeID.Equals(currentAuthor.NodeID));

                AuthorList = await Authors.ToListAsync();
            }

            return View(avm);

        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Author
                .Include(j => j.Journals)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.AuthorID == id);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorID,Firstname,Lastname")] Author author)
        {
            if (ModelState.IsValid)
            {
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Author.SingleOrDefaultAsync(m => m.AuthorID == id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("AuthorID,Firstname,Lastname")] Author author)
        {
            if (id != author.AuthorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.AuthorID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(author);
        }

        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Author
                .SingleOrDefaultAsync(m => m.AuthorID == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var author = await _context.Author.SingleOrDefaultAsync(m => m.AuthorID == id);
            _context.Author.Remove(author);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool AuthorExists(long id)
        {
            return _context.Author.Any(e => e.AuthorID == id);
        }

        private IDictionary<Author, string> Display(List<Author> authorList, List<Node> nodeList)
        {
            IDictionary<Author, string> dictionary = new Dictionary<Author, string>();

            foreach (Author author in authorList)
            {
                foreach (Node node in nodeList)
                {
                    if (author.NodeID.Equals(node.ID))
                    {
                        dictionary.Add(author, node.Name);
                    }
                }
            }
            return dictionary;
        }
    }
}
