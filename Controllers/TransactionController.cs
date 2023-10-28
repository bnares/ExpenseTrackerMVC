using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseTrackerMVC.Models;

namespace ExpenseTrackerMVC.Controllers
{
    public class TransactionController : Controller
    {
        private readonly AppDbContext _context;

        public TransactionController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            var appDbContext = await _context.Transaction.Include(x=>x.Category).ToListAsync();
            return View( appDbContext);
        }

       

        // GET: Transaction/AddOrEdit
        public IActionResult AddOrEdit(int id =0)
        {
            PopulatedCategories();
            if (id == 0)
            {
                return View(new Transaction());
            }
            else
            {
                return View(_context.Transaction.Find(id));
            }

            
        }

        // POST: Transaction/AddOrEdit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("TransactionId,CategoryId,Amount,Note,Date")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                if(transaction.TransactionId== 0)
                {
                    _context.Transaction.Add(transaction);
                    
                }
                else
                {
                    _context.Update(transaction);
                }
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            //ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", transaction.CategoryId);
            PopulatedCategories();
            return View(transaction);
        }
        
        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Transaction == null)
            {
                return Problem("Entity set 'AppDbContext.Transactions'  is null.");
            }
            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction != null)
            {
                _context.Transaction.Remove(transaction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        public void PopulatedCategories()
        {
            var CategoriesCollecion = _context.Category.ToList();
            Category defaultCategory = new Category() { CategoryId = 0, Title = "Choose Category" };
            CategoriesCollecion.Insert(0,defaultCategory);
            ViewBag.Categories = CategoriesCollecion;
        }
    }
}
