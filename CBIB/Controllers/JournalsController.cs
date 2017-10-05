using CBIB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CBIB.Controllers
{
    public class JournalsController : Controller
    {
        private readonly CBIBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private IHostingEnvironment _environment;

        public JournalsController(CBIBContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        // GET: Journals
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Global Administrator"))
            {
                return View(await _context.Journal.ToListAsync());
            }
            var user = await _userManager.GetUserAsync(User);
            var currentAuthor = _context.Author.Find(user.AuthorID);

            var journalsList = await _context.Journal.Where(a => a.AuthorID.Equals(currentAuthor.AuthorID)).ToListAsync();

            var CoAuthorList = await _context.Journal.Where(a => a.CoAuthor1.Equals(currentAuthor.Name)).ToListAsync();

            foreach (Journal coAuthor in CoAuthorList)
            {
                journalsList.Add(coAuthor);
            }

            CoAuthorList = await _context.Journal.Where(a => a.CoAuthor2.Equals(currentAuthor.Name)).ToListAsync();

            foreach (Journal coAuthor in CoAuthorList)
            {
                journalsList.Add(coAuthor);
            }
            return View(journalsList);
        }

        // GET: Journals/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var journal = await _context.Journal
                .SingleOrDefaultAsync(m => m.ID == id);
            if (journal == null)
            {
                return NotFound();
            }

            return View(journal);
        }

        // GET: Journals/Create
        [HttpGet]
        public IActionResult Create()
        {
            List<Author> coAuthors = new List<Author>();

            coAuthors = (from Name in _context.Author select Name).OrderBy(n=>n.Name).ToList();

            coAuthors.Insert(0, new Author
            {
                AuthorID = 0,
                Name = "Select"
            });

            ViewBag.ListOfNodes = coAuthors;
            return View();
        }

        //Task Download
        public async Task<IActionResult> JournalDownload(long id)
        {
            return File((await Download(id, "JournalUrl")), "application/pdf", "Too.pdf");
        }

        [Authorize(Roles = "Global Administrator, Node Administrator")]
        public async Task<IActionResult> PeerReviewDownload(long id)
        {
            return File((await Download(id, "PeerUrl")), "application/pdf", "Too.pdf");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Type,Year,Abstract")] Journal journal, IFormFile file1, IFormFile file2, Journal listBox)
        {
            var user = await _userManager.GetUserAsync(User);
            var author = _context.Author.Find(user.AuthorID);

            if (ModelState.IsValid)
            {
                if (!listBox.CoAuthor1.Equals("0"))
                {
                    journal.CoAuthor1 = await FindAuthorName(listBox.CoAuthor1);
                }

                if (!listBox.CoAuthor2.Equals("0"))
                {
                    journal.CoAuthor2 = await FindAuthorName(listBox.CoAuthor2);
                }
                    
                journal.AuthorID = user.AuthorID;

                if (file1 != null)
                {
                    journal.url = await Upload(file1);
                }

                if (file2 != null)
                {
                    journal.PeerUrl = await Upload(file2);
                }

                journal.ProofOfpeerReview = "This journal has not yet been peer reviewed";
                _context.Add(journal);
                await _context.SaveChangesAsync();

                author.Journals.Add(journal);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(journal);
        }

        // GET: Journals/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var journal = await _context.Journal.SingleOrDefaultAsync(m => m.ID == id);

            List<Author> coAuthors = new List<Author>();

            coAuthors = (from Name in _context.Author select Name).ToList();

            coAuthors.Insert(0, new Author
            {
                AuthorID = 0,
                Name = "Select"
            });

            ViewBag.ListOfNodes = coAuthors;

            if (journal == null)
            {
                return NotFound();
            }
            return View(journal);
        }

        // POST: Journals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,Title,Type,Year,Abstract,PeerReviewed")] Journal journal, IFormFile file1, IFormFile file2, Journal listBox)
        {
            if (id != journal.ID)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var author = _context.Author.Find(user.AuthorID);
            var currentJournal = _context.Journal.Find(id);
            _context.Entry(currentJournal).State = EntityState.Detached;

            if (ModelState.IsValid)
            {
                try
                {
                    if (!listBox.CoAuthor1.Equals("0"))
                    {
                        journal.CoAuthor1 = await FindAuthorName(listBox.CoAuthor1);
                    }
                    else
                    {
                        journal.CoAuthor1 = currentJournal.CoAuthor1;
                    }

                    if (!listBox.CoAuthor2.Equals("0"))
                    {
                        journal.CoAuthor2 = await FindAuthorName(listBox.CoAuthor2);
                    }
                    else
                    {
                        journal.CoAuthor2 = currentJournal.CoAuthor2;
                    }

                    journal.AuthorID = user.AuthorID;

                    if (file1 != null)
                    {
                        journal.url = await Upload(file1);
                    }

                    if (file2 != null)
                    {
                        journal.PeerUrl = await Upload(file2);
                    }

                    if(journal.PeerReviewed)
                    {
                        journal.ProofOfpeerReview = "This journal has been peer reviewed";
                    }
                    else
                    {
                        journal.ProofOfpeerReview = currentJournal.ProofOfpeerReview;
                    }

                    _context.Update(journal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JournalExists(journal.ID))
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
            return View(journal);
        }

        private Task<string> FindAuthorName(Task<string> task)
        {
            throw new NotImplementedException();
        }

        // GET: Journals/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var journal = await _context.Journal
                .SingleOrDefaultAsync(m => m.ID == id);
            if (journal == null)
            {
                return NotFound();
            }

            return View(journal);
        }

        // POST: Journals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var journal = await _context.Journal.SingleOrDefaultAsync(m => m.ID == id);
            _context.Journal.Remove(journal);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool JournalExists(long id)
        {
            return _context.Journal.Any(e => e.ID == id);
        }

        private async Task<string> Upload(IFormFile file)
        {
            string url = "";
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");

            string fName = file.FileName;

            string fileName = fName.Substring(fName.LastIndexOf("\\")+1);

            //url = Path.Combine(uploads,fileName);
            url = uploads +"\\"+ fileName;

            using (var fileStream = new FileStream(url, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            string SignificantPath = url;
            int index = 0;
            index = SignificantPath.IndexOf("uploads");

            SignificantPath = url.Substring(index - 1);
            SignificantPath = @SignificantPath.Replace("\\", "/");

            return SignificantPath;
        }

        private async Task<string> Download(long id, string type)
        {
            var journal = from m in _context.Journal select m;

            var journalList = (await journal.ToListAsync());

            var Url = "";

            foreach (Journal j in journalList)
            {
                if (type.Equals("JournalUrl"))
                {
                    if (j.ID == id)
                    {
                        Url = j.url;
                    }
                }
                else if (type.Equals("PeerUrl"))
                {
                    if (j.ID == id)
                    {
                        Url = j.PeerUrl;
                    }
                }
            }
            return Url;
        }

        private async Task<string> FindAuthorName(string id)
        {
            string result = ((await _context.Author.FindAsync(Convert.ToInt64(id)))).Name;

            return result;
        }
    }
}