using Microsoft.AspNetCore.Mvc;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using System.Linq;

public class VolunteerController : Controller
{
    private readonly ApplicationDbContext _context;

    public VolunteerController(ApplicationDbContext context)
    {
        _context = context;
    }

    // View all volunteers
    public IActionResult Index()
    {
        var volunteers = _context.Volunteers.ToList();
        return View(volunteers);
    }

    // Volunteer sign-up (GET)
    public IActionResult SignUp()
    {
        return View();
    }

    // Volunteer sign-up (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SignUp(Volunteer model)
    {
        if (ModelState.IsValid)
        {
            _context.Volunteers.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(model);
    }
}
