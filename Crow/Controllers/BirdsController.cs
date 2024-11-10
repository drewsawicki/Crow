using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Crow.Data;
using Crow.Models;
using Crow.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Runtime.InteropServices;
using System.Drawing.Printing;

namespace Crow.Controllers
{
    public class BirdsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BirdsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Birds
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
        {

            // Get the logged-in user's ID
            var userId = _userManager.GetUserId(User);

            // Fetch the logged-in user's UserBirds
            var userBirdIds = _context.UserBird
                                    .Where(ub => ub.UserId == userId)
                                    .Select(ub => ub.BirdId)
                                    .ToList();

            ViewBag.SortOrder = sortOrder;
            ViewBag.SearchString = searchString;
            
            var birds = from b in _context.Bird select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                birds = birds.Where(b => b.CommonName.Contains(searchString) 
                                    || b.ScientificName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "com_desc":
                    birds = birds.OrderByDescending(b => b.CommonName);
                    break;
                case "sci":
                    birds = birds.OrderBy(b => b.ScientificName);
                    break;
                case "sci_desc":
                    birds = birds.OrderByDescending(b => b.ScientificName);
                    break;
                default:
                    ViewBag.SortOrder = "com";
                    birds = birds.OrderBy(b => b.CommonName);
                    break;
            }

            int pageSize = 16;
            var viewModel = new BirdIndexViewModel
            {
                Birds = await PaginatedList<Bird>.CreateAsync(birds.AsNoTracking(), pageNumber ?? 1, pageSize),
                UserBirdIds = userBirdIds
            };

            return View(viewModel);
        }



        // POST: Birds/AddToLifeList
        public async Task<IActionResult> AddToLifeList(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var existingUserBird = await _context.UserBird
                .FirstOrDefaultAsync(ub => ub.UserId == user.Id && ub.BirdId == id);

            if (existingUserBird == null)
            {
                var userBird = new UserBird
                {
                    UserId = user.Id,
                    BirdId = id
                };

                if (ModelState.IsValid)
                {
                    _context.Add(userBird);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Birds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bird = await _context.Bird
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bird == null)
            {
                return NotFound();
            }

            return View(bird);
        }

        // GET: Birds/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Birds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CommonName,ScientificName")] Bird bird)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bird);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bird);
        }

        // GET: Birds/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bird = await _context.Bird.FindAsync(id);
            if (bird == null)
            {
                return NotFound();
            }
            return View(bird);
        }

        // POST: Birds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CommonName,ScientificName")] Bird bird)
        {
            if (id != bird.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bird);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BirdExists(bird.Id))
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
            return View(bird);
        }

        // GET: Birds/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bird = await _context.Bird
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bird == null)
            {
                return NotFound();
            }

            return View(bird);
        }

        // POST: Birds/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bird = await _context.Bird.FindAsync(id);
            if (bird != null)
            {
                _context.Bird.Remove(bird);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BirdExists(int id)
        {
            return _context.Bird.Any(e => e.Id == id);
        }
    }
}
