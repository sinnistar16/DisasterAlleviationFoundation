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
    public class monetariesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        public monetariesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: monetaries
        public async Task<IActionResult> Index()
        {
            decimal totalAmountDonated = _context.monetary.Sum(d => d.amount_donated);
            ViewBag.TotalAmountDonated = totalAmountDonated;

            decimal totalExpenses = CalculateTotalExpenses();
            ViewBag.TotalExpenses = totalExpenses;

            decimal totalAvailableFunds = _context.funds.Sum(d => d.total_funds);
            ViewBag.TotalAvailableFunds = totalAvailableFunds;

            return _context.monetary != null ? 
                          View(await _context.monetary.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.monetary'  is null.");
        }

        // GET: monetaries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.monetary == null)
            {
                return NotFound();
            }

            var monetary = await _context.monetary
                .FirstOrDefaultAsync(m => m.monetary_id == id);
            if (monetary == null)
            {
                return NotFound();
            }

            return View(monetary);
        }

        // GET: monetaries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: monetaries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("monetary_id,username,amount_donated,donation_date,is_anonymous,donator,total_amount")] monetary monetary)
        {
            if (ModelState.IsValid)
            {
                // Get the currently logged-in user
                var user = await _userManager.GetUserAsync(User);

                // Assign the user's email to the username field
                monetary.username = user.Email;

                _context.Add(monetary);
                await _context.SaveChangesAsync();

                // Update the funds table by adding the purchase price
                var fundsRecord = _context.funds.FirstOrDefault();
                if (fundsRecord != null)
                {
                    fundsRecord.total_funds += monetary.amount_donated;
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(monetary);
        }

        // GET: monetaries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.monetary == null)
            {
                return NotFound();
            }

            var monetary = await _context.monetary.FindAsync(id);
            if (monetary == null)
            {
                return NotFound();
            }
            return View(monetary);
        }

        // POST: monetaries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("monetary_id,username,amount_donated,donation_date,is_anonymous,donator,total_amount")] monetary monetary)
        {
            if (id != monetary.monetary_id)
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
                    monetary.username = user.Email;

                    // Fetch the existing total_amount
                    var existingTotalAmount = await _context.monetary
                        .Where(m => m.username == monetary.username)
                        .SumAsync(m => m.amount_donated);

                    // Update the total_amount with the new donation
                    monetary.total_amount = existingTotalAmount + monetary.amount_donated;


                    _context.Update(monetary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!monetaryExists(monetary.monetary_id))
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
            return View(monetary);
        }

        // GET: monetaries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.monetary == null)
            {
                return NotFound();
            }

            var monetary = await _context.monetary
                .FirstOrDefaultAsync(m => m.monetary_id == id);
            if (monetary == null)
            {
                return NotFound();
            }

            return View(monetary);
        }

        // POST: monetaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.monetary == null)
            {
                return Problem("Entity set 'ApplicationDbContext.monetary'  is null.");
            }
            var monetary = await _context.monetary.FindAsync(id);
            if (monetary != null)
            {
                _context.monetary.Remove(monetary);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool monetaryExists(int id)
        {
          return (_context.monetary?.Any(e => e.monetary_id == id)).GetValueOrDefault();
        }

        private decimal CalculateTotalExpenses()
        {
            decimal totalExpenses = 0;

            // Calculate expenses from purchased goods
            var purchasedGoodsExpenses = _context.purchased_goods.Sum(g => g.price * g.number_of_items);

            // Calculate expenses from disasters
            var disasterExpenses = _context.disasters.Where(d => d.money_allocated_amount.HasValue).Sum(d => d.money_allocated_amount.Value);

            // Calculate the total expenses
            totalExpenses = purchasedGoodsExpenses + disasterExpenses;

            return totalExpenses;
        }

    }
}
