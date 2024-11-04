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
    public class purchased_goodsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public purchased_goodsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: purchased_goods
        public async Task<IActionResult> Index()
        {
            // Filter records where number_of_items > 0
            var goodPurchased = await _context.purchased_goods
                .Where(g => g.number_of_items > 0)
                .ToListAsync();

            int goodsPurcahedCount = _context.purchased_goods.Count();
            ViewBag.ActivegoodsDonatedCount = goodsPurcahedCount;

            int totalGoodsPurchased = _context.purchased_goods.Sum(d => d.number_of_items);
            ViewBag.TotalGoodsPurchased = totalGoodsPurchased;

            // Retrieve the names of goods purchased
            var goodsPurchased = _context.purchased_goods.ToList();
            ViewBag.GoodsPurchased = goodsPurchased;

            return View(goodPurchased);
        }

        // GET: purchased_goods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.purchased_goods == null)
            {
                return NotFound();
            }

            var purchased_goods = await _context.purchased_goods
                .FirstOrDefaultAsync(m => m.purchased_goods_id == id);
            if (purchased_goods == null)
            {
                return NotFound();
            }

            return View(purchased_goods);
        }

        // GET: purchased_goods/Create
        public IActionResult Create()
        {
            decimal totalAvailableFunds = _context.funds.Sum(d => d.total_funds);
            ViewBag.TotalAvailableFunds = totalAvailableFunds;

            return View();
        }

        // POST: purchased_goods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("purchased_goods_id,username,item_name,item_description,item_category,CustomCategoryName,number_of_items,price,purchased_date")] purchased_goods purchased_goods)
        {
            if (ModelState.IsValid)
            {
                // Get the currently logged-in user
                var user = await _userManager.GetUserAsync(User);

                // Assign the user's email to the username field
                purchased_goods.username = user.Email;

                decimal totalAvailableFunds = _context.funds.Sum(d => d.total_funds);
                ViewBag.TotalAvailableFunds = totalAvailableFunds;

                // Calculate the total cost (price * number_of_items)
                var totalCost = purchased_goods.price * purchased_goods.number_of_items;

                // Fetch the existing total_amount
                var existingTotalAmount = await _context.funds
                    .SumAsync(m => m.total_funds);

                // Check if the purchase amount is greater than the total amount
                if (totalCost <= existingTotalAmount)
                {
                    // Sufficient funds available, proceed with the purchase
                    _context.Add(purchased_goods);
                    await _context.SaveChangesAsync();

                    // Update the total_funds by subtracting the price of the purchased good
                    var funds = await _context.funds.FirstOrDefaultAsync();
                    if (funds != null)
                    {
                        funds.total_funds -= totalCost;
                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Insufficient funds, return an error message
                    ModelState.AddModelError("price", $"Insufficient funds. The total cost R{totalCost:N2} which is greater than the available funds.");
                }
            }
            return View(purchased_goods);
        }

        // GET: purchased_goods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.purchased_goods == null)
            {
                return NotFound();
            }

            var purchased_goods = await _context.purchased_goods.FindAsync(id);
            if (purchased_goods == null)
            {
                return NotFound();
            }
            return View(purchased_goods);
        }

        // POST: purchased_goods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("purchased_goods_id,username,item_name,item_description,item_category,CustomCategoryName,number_of_items,price,purchased_date")] purchased_goods purchased_goods)
        {
            if (id != purchased_goods.purchased_goods_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchased_goods);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!purchased_goodsExists(purchased_goods.purchased_goods_id))
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
            return View(purchased_goods);
        }

        // GET: purchased_goods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.purchased_goods == null)
            {
                return NotFound();
            }

            var purchased_goods = await _context.purchased_goods
                .FirstOrDefaultAsync(m => m.purchased_goods_id == id);
            if (purchased_goods == null)
            {
                return NotFound();
            }

            return View(purchased_goods);
        }

        // POST: purchased_goods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.purchased_goods == null)
            {
                return Problem("Entity set 'ApplicationDbContext.purchased_goods'  is null.");
            }
            var purchased_goods = await _context.purchased_goods.FindAsync(id);
            if (purchased_goods != null)
            {
                _context.purchased_goods.Remove(purchased_goods);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool purchased_goodsExists(int id)
        {
          return (_context.purchased_goods?.Any(e => e.purchased_goods_id == id)).GetValueOrDefault();
        }
    }
}
