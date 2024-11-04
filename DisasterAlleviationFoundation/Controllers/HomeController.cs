using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;

namespace DisasterAlleviationFoundation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            DateTime currentDate = DateTime.Now;

            int activeDisasterCount = _context.disasters.Count(d =>
        currentDate >= d.disaster_start_date && currentDate <= d.disaster_end_date);

            // Retrieve disasters that have been assigned goods or money
            var assignedGoods = _context.disasters
                .Where(d =>
                    (currentDate >= d.disaster_start_date && currentDate <= d.disaster_end_date) &&
                    d.good_allocated_name != null)
                .ToList();

            var assignedMoney = _context.disasters
                .Where(d =>
                    (currentDate >= d.disaster_start_date && currentDate <= d.disaster_end_date) &&
                    d.money_allocated_amount != null)
                .ToList();

            int goodsDonatedCount = _context.good_donations.Count();
            ViewBag.ActivegoodsDonatedCount = goodsDonatedCount;

            int goodsDonatedQuantity = _context.good_donations.Sum(d => d.number_of_items);
            ViewBag.ActivegoodsDonatedQuantity = goodsDonatedQuantity;

            decimal totalAmountDonated = _context.monetary.Sum(d => d.amount_donated);
           ViewBag.TotalAmountDonated = totalAmountDonated;

            // Retrieve active disasters
            var activeDisasters = _context.disasters.ToList();

            // Retrieve the names of goods donated
            var goodsDonated = _context.good_donations.ToList();

            // Retrieve monetary donations
            var monetaryDonations = _context.monetary.ToList();

            ViewBag.ActiveDisasters = activeDisasters;
            ViewBag.GoodsDonated = goodsDonated;
            ViewBag.MonetaryDonations = monetaryDonations;

            ViewBag.ActiveDisasterCount = activeDisasterCount;
            ViewBag.AssignedGoods = assignedGoods;
            ViewBag.AssignedMoney = assignedMoney;

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
    }
}