using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Retail_POS.Data;

namespace Retail_POS.Controllers
{
   
    public class StockController : Controller
    {

        private readonly AppDbContext _context;
        public StockController(AppDbContext context)
        {
            _context = context;
        }
        public async Task <IActionResult> Index()
        {
            var products = await _context.Products
            .Include(p => p.Category)
            .ToListAsync();

            return View(products);
        }
    }
}
