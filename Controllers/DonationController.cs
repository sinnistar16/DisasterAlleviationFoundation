using Microsoft.AspNetCore.Mvc;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using System.Linq;

public class DonationController : Controller
{
    private readonly ApplicationDbContext _context;

    public DonationController(ApplicationDbContext context)
    {
        _context = context;
    }

    // View all donations
    public IActionResult Index()
    {
        var donations = _context.Donations.ToList();
        return View(donations);
    }

    // Donate resources (GET)
    public IActionResult Donate()
    {
        return View();
    }

    // Donate resources (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Donate(Donation model)
    {
        if (ModelState.IsValid)
        {
            _context.Donations.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(model);
    }
}
