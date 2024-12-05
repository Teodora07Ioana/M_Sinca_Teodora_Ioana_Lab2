using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using M_Sinca_Teodora_Ioana_Lab2.Data;
using M_Sinca_Teodora_Ioana_Lab2.Models.LibraryViewModels;
using M_Sinca_Teodora_Ioana_Lab2.Models;

namespace M_Sinca_Teodora_Ioana_Lab2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyLibraryContext _context;


        public HomeController(MyLibraryContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ActionResult> Statistics()
        {
            IQueryable<OrderGroup> data =
            from order in _context.Order
            group order by order.OrderDate into dateGroup
            select new OrderGroup()
            {
                OrderDate = dateGroup.Key,
                BookCount = dateGroup.Count()
            };
            return View(await data.AsNoTracking().ToListAsync());
        }

        public IActionResult Index()
        {
            return View();
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

        public IActionResult Chat()
        {
            return View();
        }

        public IActionResult Notification()
        {
            return View();
        }
    }
}