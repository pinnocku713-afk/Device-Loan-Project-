using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Device_Loan_Project.Data;
using Device_Loan_Project.Models;

namespace Device_Loan_Project.Controllers
{
    public class DevicesController : Controller
    {
        private readonly AppDbContext _db;
        public DevicesController(AppDbContext db) => _db = db;


        [HttpGet("devices/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var device = await _db.Devices.FindAsync(id);
            if (device == null) return NotFound();
            return View(device);
        }

        [HttpGet("devices")]
        public async Task<IActionResult> Index(string? q, bool availableOnly)
        {
            var query = _db.Devices.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(d => d.Name.Contains(q));
            if (availableOnly)
                query = query.Where(d => d.IsOnLoan == "no");

            var devices = await query.OrderBy(d => d.AssetTag).ToListAsync();

            ViewBag.Count = devices.Count;
            ViewBag.Q = q;
            ViewBag.AvailableOnly = availableOnly;
            return View(devices);
        }


        [HttpGet("devices/add")]
        public IActionResult Add() => View(new Device());

        [HttpPost("devices/add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(
            [Bind("AssetTag,Name,Category,PurchaseYear,IsOnLoan")] Device device)
        {
            if (!string.IsNullOrWhiteSpace(device.AssetTag) &&
                await _db.Devices.AnyAsync(d => d.AssetTag == device.AssetTag))
            {
                ModelState.AddModelError(nameof(Device.AssetTag), "Device is already here,idiot");
            }

            if (!ModelState.IsValid) return View(device);   

            device.IsOnLoan = device.IsOnLoan.Trim().ToLower();
            _db.Devices.Add(device);
            await _db.SaveChangesAsync();

            TempData["Message"] = $"{device.Name} was added.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("devices/edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var device = await _db.Devices.FindAsync(id);
            if (device == null) return NotFound();
            return View(device);
        }

        [HttpPost("devices/edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("AssetTag,Name,Category,PurchaseYear,IsOnLoan")] Device input)
        {
            var device = await _db.Devices.FindAsync(id);
            if (device == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(input.AssetTag) &&
                await _db.Devices.AnyAsync(d => d.AssetTag == input.AssetTag && d.Id != id))
            {
                ModelState.AddModelError(nameof(Device.AssetTag), "Some other device has that tag.");
            }

            if (!ModelState.IsValid)
            {
                input.Id = id;
                return View(input);
            }

            device.AssetTag = input.AssetTag;
            device.Name = input.Name;
            device.Category = input.Category;
            device.PurchaseYear = input.PurchaseYear;
            device.IsOnLoan = input.IsOnLoan.Trim().ToLower();
            await _db.SaveChangesAsync();

            TempData["Message"] = $"{device.AssetTag} was updated.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("devices/delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var device = await _db.Devices.FindAsync(id);
            if (device == null) return NotFound();
            return View(device);          // opening the page deletes nothing
        }

        [HttpPost("devices/delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var device = await _db.Devices.FindAsync(id);
            if (device == null)
            {
                TempData["Message"] = "That Device aint here no more.";
                return RedirectToAction(nameof(Index));
            }

            _db.Devices.Remove(device);
            try
            {
                await _db.SaveChangesAsync();
                TempData["Message"] = $"{device.AssetTag} was deleted.";
            }
            catch (DbUpdateConcurrencyException)  
            {
                TempData["Message"] = "That Device aint here no more.";
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
