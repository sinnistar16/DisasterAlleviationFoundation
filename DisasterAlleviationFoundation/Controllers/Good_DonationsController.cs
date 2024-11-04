using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;

namespace DisasterAlleviationFoundation.Controllers
{
    public class good_donationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public good_donationsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: good_donations
        public async Task<IActionResult> Index()
        {

            // Filter records where number_of_items > 0
            var goodsDonations = await _context.good_donations
                .Where(g => g.number_of_items > 0)
                .ToListAsync();

            int goodsPurcahedCount = _context.good_donations.Count();
            ViewBag.ActivegoodsDonatedCount = goodsPurcahedCount;

            int goodsDonatedQuantity = _context.good_donations.Sum(d => d.number_of_items);
            ViewBag.ActivegoodsDonatedQuantity = goodsDonatedQuantity;

            // Retrieve the names of goods purchased
            var goodsPurchased = _context.good_donations.ToList();
            ViewBag.GoodsPurchased = goodsPurchased;

            return View(goodsDonations);
        }

        // GET: good_donations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.good_donations == null)
            {
                return NotFound();
            }

            var good_donations = await _context.good_donations
                .FirstOrDefaultAsync(m => m.good_donations_id == id);
            if (good_donations == null)
            {
                return NotFound();
            }

            return View(good_donations);
        }

        // GET: good_donations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: good_donations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("good_donations_id,username,item_name,item_description,item_category,CustomCategoryName,number_of_items,donation_date,is_anonymous,donator")] good_donations good_donations)
        {
            if (ModelState.IsValid)
            {
                // Get the currently logged-in user
                var user = await _userManager.GetUserAsync(User);

                // Assign the user's email to the username field
                good_donations.username = user.Email;

                _context.Add(good_donations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(good_donations);
        }

        // GET: good_donations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.good_donations == null)
            {
                return NotFound();
            }

            var good_donations = await _context.good_donations.FindAsync(id);
            if (good_donations == null)
            {
                return NotFound();
            }
            return View(good_donations);
        }

        // POST: good_donations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("good_donations_id,username,item_name,item_description,item_category,CustomCategoryName,number_of_items,donation_date,is_anonymous,donator")] good_donations good_donations)
        {
            if (id != good_donations.good_donations_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the currently logged-in user
                    var user = await _userManager.GetUserAsync(User);

                    // Assign the user's email to the username field
                    good_donations.username = user.Email;

                    _context.Update(good_donations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!good_donationsExists(good_donations.good_donations_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(good_donations);
        }

        // GET: good_donations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.good_donations == null)
            {
                return NotFound();
            }

            var good_donations = await _context.good_donations
                .FirstOrDefaultAsync(m => m.good_donations_id == id);
            if (good_donations == null)
            {
                return NotFound();
            }

            return View(good_donations);
        }

        // POST: good_donations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.good_donations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.good_donations'  is null.");
            }
            var good_donations = await _context.good_donations.FindAsync(id);
            if (good_donations != null)
            {
                _context.good_donations.Remove(good_donations);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool good_donationsExists(int id)
        {
          return (_context.good_donations?.Any(e => e.good_donations_id == id)).GetValueOrDefault();
        }
    }
}
