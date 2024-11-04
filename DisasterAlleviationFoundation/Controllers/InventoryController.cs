using Microsoft.AspNetCore.Mvc;
using DisasterAlleviationFoundation.Data;

namespace DisasterAlleviationFoundation.Controllers
{
    public class InventoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoryController( ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int goodsDonatedCount = _context.good_donations.Count();
            int goodsPurcahedCount = _context.purchased_goods.Count();

            int total = goodsDonatedCount + goodsPurcahedCount;
            ViewBag.ActivegoodsDonatedCount = total;

            int goodsDonatedQuantity = _context.good_donations.Sum(d => d.number_of_items);
            int goodsPurchasedQuantity = _context.purchased_goods.Sum(d => d.number_of_items);

            int total1 = goodsDonatedQuantity + goodsPurchasedQuantity;

            ViewBag.ActivegoodsDonatedQuantity = total1;

            // Retrieve the names of goods donated
            var goodsDonated = _context.good_donations.ToList();

            ViewBag.GoodsDonated = goodsDonated;

            // Retrieve the names of goods donated
            var goodsPurchased = _context.purchased_goods.ToList();

            ViewBag.GoodsPurchased = goodsPurchased;

            return View();
        }
    }
}
