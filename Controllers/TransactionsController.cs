using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab4.Data;
using Lab4.Models;

namespace Lab4.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly InitialDbContext _context;

        public TransactionsController(InitialDbContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var initialDbContext = _context.Transactions
                .Include(t => t.BookInstance)
                .ThenInclude(t => t.Book)
                .Include(t => t.Customer);
            return View(await initialDbContext.ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.BookInstance)
                .Include(t => t.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            ViewData["Books"] = new SelectList(_context.Books, "Id", "Title");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email");
            ViewBag.Status = new SelectList(Enum.GetValues(typeof(TransactionStatus))
                                      .Cast<TransactionStatus>()
                                      .Select(v => new SelectListItem
                                      {
                                          Text = v.ToString(),
                                          Value = ((int)v).ToString()
                                      }), "Value", "Text");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerId,BookInstanceId,Status,TransactionDate")] Transaction transaction)
        {

            var currentBook = _context.Books.Where(x => x.Id == transaction.BookInstanceId).FirstOrDefault();
            transaction.BookInstance = new BookInstance
            {
                BookId = transaction.BookInstanceId,
                Book = currentBook,
            };

            transaction.Customer = _context.Customers.Where(x => x.Id == transaction.CustomerId).FirstOrDefault();

            ModelState.Remove("Customer");
            ModelState.Remove("BookInstance");
            //_context.Books.Where(x => x?.Id == transaction?.BookInstanceId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Books"] = new SelectList(_context.Books, "Id", "Title");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email");
            ViewBag.Status = new SelectList(Enum.GetValues(typeof(TransactionStatus))
                                      .Cast<TransactionStatus>()
                                      .Select(v => new SelectListItem
                                      {
                                          Text = v.ToString(),
                                          Value = ((int)v).ToString()
                                      }), "Value", "Text");

            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["BookInstanceId"] = new SelectList(_context.BookInstances, "Id", "Id", transaction.BookInstanceId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email", transaction.CustomerId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,BookInstanceId,Status,TransactionDate")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
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
            ViewData["BookInstanceId"] = new SelectList(_context.BookInstances, "Id", "Id", transaction.BookInstanceId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email", transaction.CustomerId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.BookInstance)
                .Include(t => t.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Transactions == null)
            {
                return Problem("Entity set 'InitialDbContext.Transactions'  is null.");
            }
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
          return (_context.Transactions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
