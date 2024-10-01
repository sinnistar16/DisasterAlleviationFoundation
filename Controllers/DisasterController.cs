using Microsoft.AspNetCore.Mvc;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using System.Linq;

public class DisasterController : Controller
{
    // Correctly declare _context
    private readonly ApplicationDbContext _context;

    // Fix constructor to assign _context correctly
    public DisasterController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Display all disaster incidents
    public IActionResult Index()
    {
        var disasters = _context.Disasters.ToList(); // Fetch disasters
        return View(disasters); // Pass to the view
    }

    // Create a new disaster incident report (GET)
    public IActionResult Create()
    {
        return View(); // Return the form view
    }

    // Create a new disaster incident report (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Disaster model)
    {
        if (ModelState.IsValid)
        {
            _context.Disasters.Add(model); // Add the new disaster
            _context.SaveChanges(); // Save changes to the database
            return RedirectToAction("Index"); // Redirect to the list
        }
        return View(model); // Return the form with the model if validation fails
    }
}
