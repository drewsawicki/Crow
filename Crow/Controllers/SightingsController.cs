using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Crow.Data;
using Crow.Models;
using Microsoft.AspNetCore.Identity;

namespace Crow.Controllers
{
    public class SightingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SightingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Sightings
        public async Task<IActionResult> Index(int Id)
        {
            var sightings = await _context.Sighting
                .Where(s => s.UserBirdId == Id)
                .Include(s => s.UserBird.Bird)
                .ToListAsync();

            ViewBag.UserBirdId = Id;
            return View(sightings);
        }

        // GET: Sightings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sighting = await _context.Sighting
                .Include(s => s.UserBird)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sighting == null)
            {
                return NotFound();
            }

            return View(sighting);
        }

        // GET: Sightings/Create
        public IActionResult Create()
        {
            ViewData["UserBirdId"] = new SelectList(_context.UserBird, "Id", "Id");
            return View();
        }

        // POST: Sightings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserBirdId,Location,Notes")] Sighting sighting)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var userBird = await _context.UserBird
                .FirstOrDefaultAsync(ub => ub.Id == sighting.UserBirdId);

            if (userBird == null)
            {
                return NotFound();
            }

            sighting.UserBird = userBird;

            _context.Entry(userBird).State = EntityState.Unchanged;

            if (ModelState.IsValid)
            {
                _context.Add(sighting);
                await _context.SaveChangesAsync();
            }
           
            ViewData["UserBirdId"] = new SelectList(_context.UserBird, "Id", "Id", sighting.UserBirdId);
            return View(sighting);
        }

        // GET: Sightings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sighting = await _context.Sighting.FindAsync(id);
            if (sighting == null)
            {
                return NotFound();
            }
            ViewData["UserBirdId"] = new SelectList(_context.UserBird, "Id", "Id", sighting.UserBirdId);
            return View(sighting);
        }

        // POST: Sightings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserBirdId,Date,Time,Location,Notes")] Sighting sighting)
        {
            if (id != sighting.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sighting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SightingExists(sighting.Id))
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
            ViewData["UserBirdId"] = new SelectList(_context.UserBird, "Id", "Id", sighting.UserBirdId);
            return View(sighting);
        }

        // GET: Sightings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sighting = await _context.Sighting
                .Include(s => s.UserBird)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sighting == null)
            {
                return NotFound();
            }

            return View(sighting);
        }

        // POST: Sightings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sighting = await _context.Sighting.FindAsync(id);
            if (sighting != null)
            {
                _context.Sighting.Remove(sighting);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SightingExists(int id)
        {
            return _context.Sighting.Any(e => e.Id == id);
        }
    }
}
