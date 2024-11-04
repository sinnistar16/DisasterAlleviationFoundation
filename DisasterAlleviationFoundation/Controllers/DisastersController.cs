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
using Moq;

namespace DisasterAlleviationFoundation.Controllers
{
    public class disastersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public disastersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: disasters
        public async Task<IActionResult> Index()
        {

            DateTime currentDate = DateTime.Now;

            int activeDisasterCount = _context.disasters.Count(d =>
            currentDate >= d.disaster_start_date && currentDate <= d.disaster_end_date);

            int pendingDisasterCount = _context.disasters.Count(d =>
            currentDate < d.disaster_start_date);

            int concludedDisasterCount = _context.disasters.Count(d =>
            currentDate > d.disaster_end_date);

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

            ViewBag.AssignedGoods = assignedGoods;
            ViewBag.AssignedMoney = assignedMoney;

            ViewBag.ActiveDisasterCount = activeDisasterCount;
            ViewBag.PendingDisasterCount = pendingDisasterCount;
            ViewBag.ConcludedDisasterCount = concludedDisasterCount;


            return _context.disasters != null ?
                          View(await _context.disasters.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.disasters'  is null.");
        }

        // GET: disasters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.disasters == null)
            {
                return NotFound();
            }

            var disasters = await _context.disasters
                .FirstOrDefaultAsync(m => m.disaster_id == id);
            if (disasters == null)
            {
                return NotFound();
            }

            return View(disasters);
        }

        // GET: disasters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: disasters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("disaster_id,username,disaster_location,disaster_description,disaster_aid_type,disaster_start_date,disaster_end_date")] disasters disasters)
        {
            if (ModelState.IsValid)
            {
                // Get the currently logged-in user
                var user = await _userManager.GetUserAsync(User);

                // Assign the user's email to the username field
                disasters.username = user.Email;

                _context.Add(disasters);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(disasters);
        }

        // GET: disasters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.disasters == null)
            {
                return NotFound();
            }

            var disasters = await _context.disasters.FindAsync(id);
            if (disasters == null)
            {
                return NotFound();
            }
            return View(disasters);
        }

        // POST: disasters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("disaster_id,username,disaster_location,disaster_description,disaster_aid_type,disaster_start_date,disaster_end_date,money_allocated_amount,money_allocated_date,username_money_allocated,good_allocated_name,good_allocated_number_of_items,good_allocation_date,username_good_allocated")] disasters disasters)
        {
            if (id != disasters.disaster_id)
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
                    disasters.username = user.Email;

                    _context.Update(disasters);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!disastersExists(disasters.disaster_id))
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
            return View(disasters);
        }

        // GET: disasters/Edit/5
        public async Task<IActionResult> EditAssigningGoods(int? id)
        {
            if (id == null || _context.disasters == null)
            {
                return NotFound();
            }

            var disasters = await _context.disasters.FindAsync(id);
            if (disasters == null)
            {
                return NotFound();
            }

            // Load the names of goods from the "goods donations" and "purchased donations" and set them in ViewBag
            var goodNames = await _context.good_donations
                .Select(g => g.item_name)
                .Concat(_context.purchased_goods.Select(p => p.item_name))
                .Distinct()
                .ToListAsync();

            ViewBag.GoodNames = goodNames;

            // Fetch the maximum quantities for each item from the same table based on the selected item
            var maxQuantities = await _context.good_donations
                .Where(item => goodNames.Contains(item.item_name))
                .ToDictionaryAsync(item => item.item_name, item => item.number_of_items);

            ViewBag.MaxQuantities = maxQuantities;

            return View(disasters);
        }

        // POST: disasters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAssigningGoods(int id, [Bind("disaster_id,username,disaster_location,disaster_description,disaster_aid_type,disaster_start_date,disaster_end_date,money_allocated_amount,money_allocated_date,username_money_allocated,good_allocated_name,good_allocated_number_of_items,good_allocation_date,username_good_allocated")] disasters disasters)
        {
            if (id != disasters.disaster_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the currently logged-in user
                    var user = await _userManager.GetUserAsync(User);
                    var user1 = await _userManager.GetUserAsync(User);


                    // Assign the user's email to the username field
                    disasters.username_good_allocated = user.Email;

                    // Assign the user's email to the username field
                    disasters.username = user1.Email;

                    // Fetch the maximum quantities for each item from your database
                    var maxQuantities = await _context.good_donations
                        .ToDictionaryAsync(item => item.item_name, item => item.number_of_items);

                    ViewBag.MaxQuantities = maxQuantities;

                    // Determine the source table of the allocated item (good_donations or purchased_goods)
                    string sourceTable;
                    int allocatedQuantity;

                    // Check if the item exists in good_donations
                    var goodDonationItem = await _context.good_donations
                        .Where(g => g.item_name == disasters.good_allocated_name)
                        .FirstOrDefaultAsync();

                    if (goodDonationItem != null)
                    {
                        sourceTable = "good_donations";
                        allocatedQuantity = disasters.good_allocated_number_of_items;
                    }
                    else
                    {
                        // Check if the item exists in purchased_goods
                        var purchasedGoodsItem = await _context.purchased_goods
                            .Where(p => p.item_name == disasters.good_allocated_name)
                            .FirstOrDefaultAsync();

                        if (purchasedGoodsItem != null)
                        {
                            sourceTable = "purchased_goods";
                            allocatedQuantity = disasters.good_allocated_number_of_items;

                            // Subtract the allocated quantity from purchased_goods
                            purchasedGoodsItem.number_of_items -= allocatedQuantity;
                            _context.Update(purchasedGoodsItem);
                        }
                        else
                        {
                            // The item does not exist in either table, handle this case as needed
                            ModelState.AddModelError("good_allocated_name", "Item not found in donations or purchased items.");
                            return View(disasters);
                        }
                    }

                    if (sourceTable == "good_donations")
                    {
                        // Subtract the allocated quantity from good_donations
                        goodDonationItem.number_of_items -= allocatedQuantity;
                        // Update the purchased goods in the database
                        _context.Update(goodDonationItem);
                    }

                    _context.Update(disasters);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!disastersExists(disasters.disaster_id))
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
            return View(disasters);
        }

        // GET: disasters/Edit/5
        public async Task<IActionResult> EditAssigningMoney(int? id)
        {
            if (id == null || _context.disasters == null)
            {
                return NotFound();
            }

            decimal totalAvailableFunds = _context.funds.Sum(d => d.total_funds);
            ViewBag.TotalAvailableFunds = totalAvailableFunds;

            var disasters = await _context.disasters.FindAsync(id);
            if (disasters == null)
            {
                return NotFound();
            }
            return View(disasters);
        }

        // POST: disasters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Route("disasters/EditAssigningMoney")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAssigningMoney(int id, [Bind("disaster_id,username,disaster_location,disaster_description,disaster_aid_type,disaster_start_date,disaster_end_date,money_allocated_amount,money_allocated_date,username_money_allocated,good_allocated_name,good_allocated_number_of_items,good_allocation_date,username_good_allocated")] disasters disasters)
        {

            if (ModelState.IsValid)
            {
                // Get the currently logged-in user
                var user = await _userManager.GetUserAsync(User);
                var user1 = await _userManager.GetUserAsync(User);


                // Assign the user's email to the username field
                disasters.username_money_allocated = user.Email;

                // Assign the user's email to the username field
                disasters.username = user1.Email;
                // Assign the user's email to the username field
                disasters.username_money_allocated = user.Email;
                // Assign the user's email to the username field
                disasters.username = user1.Email;

                decimal totalAvailableFunds = _context.funds.Sum(d => d.total_funds);
                ViewBag.TotalAvailableFunds = totalAvailableFunds;

                // Fetch the existing total_amount
                var existingTotalAmount = await _context.funds
                        .SumAsync(m => m.total_funds);

                // Check if the purchase amount is greater than the total amount
                if (disasters.money_allocated_amount <= existingTotalAmount)
                {
                    _context.Update(disasters);
                    await _context.SaveChangesAsync();

                    // Update the total_funds by subtracting the price of the purchased good
                    var funds = await _context.funds.FirstOrDefaultAsync();
                    if (funds != null)
                    {
                        //funds.total_funds -= disasters.money_allocated_amount;
                        await _context.SaveChangesAsync();
                    }
                    // Update the total_funds by subtracting the price of the purchased good
                    var funds1 = await _context.funds.FirstOrDefaultAsync();
                    if (funds1 != null)
                    {
                        int moneyAllocatedAmount = disasters.money_allocated_amount ?? 0;

                        // Subtract the allocated money from available funds
                        funds1.total_funds -= moneyAllocatedAmount;

                        // Update the disaster and available funds in the database
                        _context.Update(funds1);

                        //funds.total_funds -= disasters.money_allocated_amount;
                        await _context.SaveChangesAsync();
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Insufficient funds, return an error message
                    ModelState.AddModelError("price", "Insufficient funds");
                }
            }
            return View(disasters);
        }

        // GET: disasters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.disasters == null)
            {
                return NotFound();
            }

            var disasters = await _context.disasters
                .FirstOrDefaultAsync(m => m.disaster_id == id);
            if (disasters == null)
            {
                return NotFound();
            }

            return View(disasters);
        }

        // POST: disasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.disasters == null)
            {
                return Problem("Entity set 'ApplicationDbContext.disasters'  is null.");
            }
            var disasters = await _context.disasters.FindAsync(id);
            if (disasters != null)
            {
                _context.disasters.Remove(disasters);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool disastersExists(int id)
        {
            return (_context.disasters?.Any(e => e.disaster_id == id)).GetValueOrDefault();
        }

        // Add this action to fetch maximum quantities
        [HttpGet]
        public async Task<IActionResult> GetMaxQuantities()
        {
            // Retrieve the maximum quantities for items from goods donations
            var maxQuantities = await _context.good_donations
                .GroupBy(g => g.item_name)
                .Select(g => new
                {
                    ItemName = g.Key,
                    MaxQuantity = g.Sum(x => x.number_of_items)
                })
                .ToDictionaryAsync(x => x.ItemName, x => x.MaxQuantity);

            // Retrieve the maximum quantities for items from purchased goods
            var maxQuantitiesPurchased = await _context.purchased_goods
                .GroupBy(p => p.item_name)
                .Select(p => new
                {
                    ItemName = p.Key,
                    MaxQuantity = p.Sum(x => x.number_of_items)
                })
                .ToDictionaryAsync(x => x.ItemName, x => x.MaxQuantity);

            // Combine the maximum quantities from both sources
            foreach (var kvp in maxQuantitiesPurchased)
            {
                if (maxQuantities.ContainsKey(kvp.Key))
                {
                    maxQuantities[kvp.Key] += kvp.Value;
                }
                else
                {
                    maxQuantities[kvp.Key] = kvp.Value;
                }
            }

            return Json(maxQuantities);
        }

        public Task<object> EditAssigningGoodsTest(int disasterId, UserManager<IdentityUser> @object, string v1, string v2, string goodAllocatedName1, DateTime disasterStartDate, DateTime disasterEndDate, int moneyAllocatedAmount, DateTime moneyAllocatedDate, string usernameMoneyAllocated, string goodAllocatedName2, int goodAllocatedNumberOfItems, DateTime goodAllocationDate, string usernameGoodAllocated)
        {
            return Task.FromResult<object>(result: EditAssigningGoodsTest);
        }
    }
}
