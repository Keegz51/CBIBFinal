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
            return View(await _context.Journal.ToListAsync());
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
        public IActionResult Create()
        {
            return View();
        }

        //Task Download
        public async Task<IActionResult> JournalDownload(long id)
        {
            return File((await Download(id, "JournalUrl")), "application/pdf", "Too.pdf");
        }

        [Authorize(Roles = "Global Administrator,  Node Administrator")]
        public async Task<IActionResult> PeerReviewDownload(long id)
        {
            return File((await Download(id, "PeerUrl")), "application/pdf", "Too.pdf");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Type,CoAuthor1,CoAuthor2,Year,Abstract")] Journal journal, IFormFile file1, IFormFile file2)
        {
            var user = await _userManager.GetUserAsync(User);
            var author = _context.Author.Find(user.AuthorID);

            if (ModelState.IsValid)
            {
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
        public async Task<IActionResult> Edit(long id, [Bind("ID,Title,Type,CoAuthor1,CoAuthor2,Year,Abstract,PeerReviewed")] Journal journal, IFormFile file1, IFormFile file2)
        {
            if (id != journal.ID)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var author = _context.Author.Find(user.AuthorID);

            if (ModelState.IsValid)
            {
                try
                {
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

            url = Path.Combine(uploads, file.FileName);

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
    }
}