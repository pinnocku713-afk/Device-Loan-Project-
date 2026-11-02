using Device_Loan_Project.Data;
using Device_Loan_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Device_Loan_Project.Controllers
{
    public class DevicesController : Controller
    {
        private readonly AppDbContext _db;

        public DevicesController(AppDbContext db)
        {
            _db = db;
        }

        // Task 3 + Task 5: list, search and filter
        [HttpGet("devices")]
        public IActionResult Index(string? q, bool availableOnly)
        {
            var query = _db.Devices.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(d => d.Name.Contains(q));
            if (availableOnly)
                query = query.Where(d => d.IsOnLoan == "no");

            var devices = query.OrderBy(d => d.AssetTag).ToList();

            ViewBag.Count = devices.Count;
            ViewBag.Q = q;
            ViewBag.AvailableOnly = availableOnly;
            return View(devices);
        }

        // Task 4: details
        [HttpGet("devices/{id:int}")]
        public IActionResult Details(int id)
        {
            Device? device = _db.Devices.Find(id);
            if (device == null) return NotFound();
            return View(device);
        }

        // Task 6: add
        [HttpGet("devices/add")]
        public IActionResult Add()
        {
            return View(new Device { PurchaseYear = DateTime.Now.Year });
        }

        [HttpPost("devices/add")]
        [ValidateAntiForgeryToken]
        public IActionResult Add(
            [Bind("AssetTag,Name,Category,PurchaseYear,IsOnLoan")] Device device)
        {
            if (!string.IsNullOrWhiteSpace(device.AssetTag) &&
                _db.Devices.Any(d => d.AssetTag == device.AssetTag))
            {
                ModelState.AddModelError(nameof(Device.AssetTag), "Another device already has that asset tag.");
            }

            if (!ModelState.IsValid) return View(device);

            device.IsOnLoan = device.IsOnLoan.Trim().ToLower();
            _db.Devices.Add(device);
            _db.SaveChanges();

            TempData["Message"] = $"{device.AssetTag} {device.Name} was added.";
            return RedirectToAction("Index");
        }

        // Task 7: edit
        [HttpGet("devices/edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            Device? device = _db.Devices.Find(id);
            if (device == null) return NotFound();
            return View(device);
        }

        [HttpPost("devices/edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,
            [Bind("AssetTag,Name,Category,PurchaseYear,IsOnLoan")] Device input)
        {
            Device? device = _db.Devices.Find(id);
            if (device == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(input.AssetTag) &&
                _db.Devices.Any(d => d.AssetTag == input.AssetTag && d.Id != id))
            {
                ModelState.AddModelError(nameof(Device.AssetTag), "Another device already has that asset tag.");
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
            _db.SaveChanges();

            TempData["Message"] = $"{device.AssetTag} was updated.";
            return RedirectToAction("Index");
        }

        // Task 8: delete
        [HttpGet("devices/delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            Device? device = _db.Devices.Find(id);
            if (device == null) return NotFound();
            return View(device);
        }

        [HttpPost("devices/delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Device? device = _db.Devices.Find(id);
            if (device == null)
            {
                TempData["Message"] = "That device was already deleted.";
                return RedirectToAction("Index");
            }

            _db.Devices.Remove(device);
            try
            {
                _db.SaveChanges();
                TempData["Message"] = $"{device.AssetTag} was deleted.";
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["Message"] = "That device was already deleted.";
            }
            return RedirectToAction("Index");
        }
    }
}
