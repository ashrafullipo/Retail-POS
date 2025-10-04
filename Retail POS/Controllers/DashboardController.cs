using Microsoft.AspNetCore.Mvc;
using Retail_POS.Data;

namespace Retail_POS.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext context;

        public DashboardController (AppDbContext context)
        {
            this.context = context;
        }



        public IActionResult Index()
        {
            var totalProducts = context.Products.Count();
            var totalCustomer = context.Customers.Count();
            var totalSuppliers = context.Suppliers.Count();
            var totalSales = context.Sales.Count();


            ViewBag.TotalProducts = totalProducts;  
            ViewBag.TotalCustomers = totalCustomer;
            ViewBag.TotalSuppliers = totalSuppliers;
            ViewBag.TotalSales = totalSales;


            return View();
        }
    }
}
