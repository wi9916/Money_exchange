using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Money_exchange.Models;
using Microsoft.EntityFrameworkCore;

namespace Money_exchange.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationContext db;

        public HomeController(ApplicationContext context)
        {
            db = context;
        }
        
        public async Task<IActionResult> Index(Operation operation)
        {
            if (operation.ToAmount > 0)
            {
                operation.Date = DateTime.Now;
                db.Operations.Add(operation);
                await db.SaveChangesAsync();               
            }
            return View(operation);
        }
        

        public async Task<IActionResult> Oper(SortState sortOrder = SortState.DateAsc)
        {
            IQueryable<Operation> operations = db.Operations;

            ViewData["DateSort"] = sortOrder==SortState.DateAsc ? SortState.DateDesc : SortState.DateAsc;
            ViewData["FromAmountSort"] = sortOrder == SortState.FromAmountAsc ? SortState.FromAmountDesc : SortState.FromAmountAsc;
            ViewData["FromCurrencySort"] = sortOrder == SortState.FromCurrencyAsc ? SortState.FromCurrencyDesc : SortState.FromCurrencyAsc;
            ViewData["ToAmountSort"] = sortOrder == SortState.ToAmountAsc ? SortState.ToAmountDesc : SortState.ToAmountAsc;
            ViewData["ToCurrencySort"] = sortOrder == SortState.ToCurrencyAsc ? SortState.ToCurrencyDesc : SortState.ToCurrencyAsc;

            operations = sortOrder switch
            {                
                SortState.DateDesc => operations.OrderByDescending(s => s.Date),
                SortState.FromAmountDesc => operations.OrderByDescending(s => s.FromAmount),
                SortState.FromAmountAsc => operations.OrderBy(s => s.FromAmount),
                SortState.FromCurrencyDesc => operations.OrderByDescending(s => s.FromCurrency),
                SortState.FromCurrencyAsc => operations.OrderBy(s => s.FromCurrency),
                SortState.ToAmountDesc => operations.OrderByDescending(s => s.ToAmount),
                SortState.ToAmountAsc => operations.OrderBy(s => s.ToAmount),
                SortState.ToCurrencyDesc => operations.OrderByDescending(s => s.ToCurrency),
                SortState.ToCurrencyAsc => operations.OrderBy(s => s.ToCurrency),
                _ => operations.OrderBy(s => s.Date),
            };
            return View(await operations.AsNoTracking().ToListAsync());
        }


        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> Create(Operation operation)
        {
            operation.Date = DateTime.Now;
            db.Operations.Add(operation);
            await db.SaveChangesAsync();

            return View(operation);            
        }
        
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Operation operation = await db.Operations.FirstOrDefaultAsync(p => p.Id == id);
                if (operation != null)                  
                return View(operation);
            }
            return NotFound();
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Operation operation = await db.Operations.FirstOrDefaultAsync(p => p.Id == id);
                if (operation != null)
                {
                    db.Operations.Remove(operation);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Oper");
                }
            }
            return NotFound();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
