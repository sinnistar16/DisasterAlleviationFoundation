using Microsoft.AspNetCore.Mvc;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models
;
using System.Linq;

public class DisasterController : Controller
{
    private readonly DisasterAlleviationFoundation.Data.ApplicationDbContext;

    public DisasterController(DisasterAlleviationFoundation.Data.ApplicationDbContext)
    {
        _context = context;
    }

    // Display all disaster incidents
    public IActionResult Index()
    {
        var disasters = _context.Disasters.ToList();
        return View(disasters);
    }

    // Create a new disaster incident report (GET)
    public IActionResult Create()
    {
        return View();
    }

    // Create a new disaster incident report (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Disaster model)
    {
        if (ModelState.IsValid)
        {
            _context.Disasters.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(model);
    }
}
