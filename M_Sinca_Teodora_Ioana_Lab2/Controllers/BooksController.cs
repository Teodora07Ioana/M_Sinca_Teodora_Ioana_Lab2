﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using M_Sinca_Teodora_Ioana_Lab2.Data;
using M_Sinca_Teodora_Ioana_Lab2.Models;

namespace M_Sinca_Teodora_Ioana_Lab2.Controllers
{
    public class BooksController : Controller
    {
        private readonly M_Sinca_Teodora_Ioana_Lab2Context _context;

        public BooksController(M_Sinca_Teodora_Ioana_Lab2Context context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            var books = from b in _context.Book
                        join a in _context.Author on b.AuthorID equals a.ID
                        select new BookViewModel
                        {
                            ID = b.ID,
                            Title = b.Title,
                            Price = b.Price,
                            FullName = a.FullName
                        };
            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Title.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "title_desc":
                    books = books.OrderByDescending(b => b.Title);
                    break;
                case "Price":
                    books = books.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    books = books.OrderByDescending(b => b.Price);
                    break;
                default:
                    books = books.OrderBy(b => b.Title);
                    break;
            }
            int pageSize = 2;
            return View(await PaginatedList<BookViewModel>.CreateAsync(books.AsNoTracking(),pageNumber ?? 1, pageSize));
           
           //return View(await books.AsNoTracking().ToListAsync());
           /*  var books = await _context.Book
                 .Include(b => b.Author)
                 .Include(b => b.Genre)
                 .ToListAsync();

              return View(books);*/
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book.Include(b => b.Genre).Include(b => b.Author).Include(s => s.Orders).ThenInclude(e => e.Customer).FirstOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["GenreID"] = new SelectList(_context.Set<Genre>(), "ID", "Name");
            ViewData["AuthorID"] = new SelectList(
                _context.Set<Author>().Select(a => new
                {
                    ID = a.ID,
                    FullName = a.FirstName + " " + a.LastName
                }),
                "ID",
                "FullName"
            );
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,AuthorID,Price,GenreID")] Book book)
        {
            try
            { 
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreID"] = new SelectList(_context.Set<Genre>(), "ID", "Name", book.GenreID);
            ViewData["AuthorID"] = new SelectList(
                _context.Set<Author>().Select(a => new
                {
                    ID = a.ID,
                    FullName = a.FirstName + " " + a.LastName
                }),
                "ID",
                "FullName"
            );
            }
            catch (DbUpdateException /* ex*/)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists ");
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["GenreID"] = new SelectList(_context.Set<Genre>(), "ID", "Name", book.GenreID);
            ViewData["AuthorID"] = new SelectList(
                 _context.Set<Author>().Select(a => new
                 {
                     ID = a.ID,
                     FullName = a.FirstName + " " + a.LastName
                 }),
                 "ID",
                 "FullName"
             );
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,AuthorID,Price,GenreID")] Book book)
        {
            if (id != book.ID)
            {
                return NotFound();
            }

            var bookToUpdate = await _context.Book.FirstOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<Book>(
             bookToUpdate,
             "",
             s => s.AuthorID, s => s.Title, s => s.Price))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreID"] = new SelectList(_context.Set<Genre>(), "ID", "Name", book.GenreID);
            //ViewData["AuthorID"] = new SelectList(
            // _context.Set<Author>().Select(a => new
            //{
            //  ID = a.ID,
            //FullName = a.FirstName + " " + a.LastName
            //}),
            //"ID",
            //"FullName"
            //);

            //return View(book);
            ViewData["AuthorID"] = new SelectList(_context.Author, "ID", "FullName",
bookToUpdate.AuthorID);
            return View(bookToUpdate);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Genre).Include(b => b.Author).AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                "Delete failed. Try again";
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Book == null)
            {
                return Problem("Entity set 'M_Sinca_Teodora_Ioana_Lab2Context.Book'  is null.");
            }
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return RedirectToAction(nameof(Index));
            }
            /*
            if (book != null)
            {
                _context.Book.Remove(book);
            }
            */
            try
            {
                _context.Book.Remove(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {

                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool BookExists(int id)
        {
          return (_context.Book?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
