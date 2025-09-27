using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Retail_POS.Data;
using Retail_POS.Models;

namespace Retail_POS.Controllers
{
    public class PurchaseItemsController : Controller
    {
        private readonly AppDbContext _context;

        public PurchaseItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PurchaseItems
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.PurchaseItems.Include(p => p.Product).Include(p => p.Purchase);
            return View(await appDbContext.ToListAsync());
        }

        // GET: PurchaseItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseItem = await _context.PurchaseItems
                .Include(p => p.Product)
                .Include(p => p.Purchase)
                .FirstOrDefaultAsync(m => m.PurchaseItemId == id);
            if (purchaseItem == null)
            {
                return NotFound();
            }

            return View(purchaseItem);
        }

        // GET: PurchaseItems/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name");
            ViewData["PurchaseId"] = new SelectList(_context.Purchases, "PurchaseId", "PurchaseId");
            return View();
        }

        // POST: PurchaseItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PurchaseItemId,PurchaseId,ProductId,Quantity,UnitPrice")] PurchaseItem purchaseItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchaseItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", purchaseItem.ProductId);
            ViewData["PurchaseId"] = new SelectList(_context.Purchases, "PurchaseId", "PurchaseId", purchaseItem.PurchaseId);
            return View(purchaseItem);
        }

        // GET: PurchaseItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseItem = await _context.PurchaseItems.FindAsync(id);
            if (purchaseItem == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", purchaseItem.ProductId);
            ViewData["PurchaseId"] = new SelectList(_context.Purchases, "PurchaseId", "PurchaseId", purchaseItem.PurchaseId);
            return View(purchaseItem);
        }

        // POST: PurchaseItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PurchaseItemId,PurchaseId,ProductId,Quantity,UnitPrice")] PurchaseItem purchaseItem)
        {
            if (id != purchaseItem.PurchaseItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchaseItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseItemExists(purchaseItem.PurchaseItemId))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", purchaseItem.ProductId);
            ViewData["PurchaseId"] = new SelectList(_context.Purchases, "PurchaseId", "PurchaseId", purchaseItem.PurchaseId);
            return View(purchaseItem);
        }

        // GET: PurchaseItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseItem = await _context.PurchaseItems
                .Include(p => p.Product)
                .Include(p => p.Purchase)
                .FirstOrDefaultAsync(m => m.PurchaseItemId == id);
            if (purchaseItem == null)
            {
                return NotFound();
            }

            return View(purchaseItem);
        }

        // POST: PurchaseItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchaseItem = await _context.PurchaseItems.FindAsync(id);
            if (purchaseItem != null)
            {
                _context.PurchaseItems.Remove(purchaseItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseItemExists(int id)
        {
            return _context.PurchaseItems.Any(e => e.PurchaseItemId == id);
        }
    }
}
