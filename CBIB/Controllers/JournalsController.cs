using CBIB.Models;
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
        public async Task<IActionResult> TaskDownload(long id)
        {
            var journal = from m in _context.Journal select m;

            var journalList = (await journal.ToListAsync());

            var journalUrl ="";

            foreach (Journal j in journalList)
            {
                if (j.ID==id)
                {
                    journalUrl = j.url;        
                }
            }
            return File(journalUrl, "application/pdf", "Too.pdf");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Year,Abstract")] Journal journal, ICollection<IFormFile> files)
        {
            string url = "";
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    url = Path.Combine(uploads, file.FileName);
                   
                    using (var fileStream = new FileStream(url, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }

            string SignificantPath = url;
            int index = 0;
            index = SignificantPath.IndexOf("uploads");

            SignificantPath = url.Substring(index-1);
            SignificantPath = @SignificantPath.Replace("\\","/");

            url = SignificantPath;

            var user = await _userManager.GetUserAsync(User);
            var author = _context.Author.Find(user.AuthorID);

            if (ModelState.IsValid)
            {
                journal.AuthorID = user.AuthorID;
                journal.url = url;
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
        public async Task<IActionResult> Edit(long id, [Bind("ID,Title,Year,AuthorID")] Journal journal)
        {
            if (id != journal.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
    }
}
