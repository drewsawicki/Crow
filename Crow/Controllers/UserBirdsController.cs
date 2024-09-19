using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Crow.Data;
using Crow.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Crow.Controllers
{
    public class UserBirdsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserBirdsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: UserBirds
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
           
            ViewBag.CommonSortParam = String.IsNullOrEmpty(sortOrder) ? "com_desc" : "";
            ViewBag.ScientificSortParam = sortOrder == "sci" ? "sci_desc" : "sci";
            var userBirds = from ub in _context.UserBird select ub;

            if (!String.IsNullOrEmpty(searchString))
            {
                userBirds = userBirds
                    .Include(ub => ub.Bird)
                    .Where(ub => ub.UserId == user.Id
                                 && (ub.Bird.CommonName.Contains(searchString)
                                 || ub.Bird.ScientificName.Contains(searchString)));
            }
            else
            {
                userBirds = userBirds
                    .Include(ub => ub.Bird)
                    .Where(ub => ub.UserId == user.Id);
            }

            switch (sortOrder)
            {
                case "com_desc":
                    userBirds = userBirds.OrderByDescending(ub => ub.Bird.CommonName);
                    break;
                case "sci":
                    userBirds = userBirds.OrderBy(ub => ub.Bird.ScientificName);
                    break;
                case "sci_desc":
                    userBirds = userBirds.OrderByDescending(ub => ub.Bird.ScientificName);
                    break;
                default:
                    userBirds = userBirds.OrderBy(ub => ub.Bird.CommonName);
                    break;
            }

            return View(userBirds);
        }

        // GET: Birds/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // POST: Birds/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            return View("Index", await _context.UserBird
                .Where(ub => ub.Bird.CommonName.Contains(SearchPhrase) && ub.UserId == user.Id)
                .Include(ub => ub.Bird)
                .ToListAsync());
        }

        // GET: UserBirds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBird = await _context.UserBird
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userBird == null)
            {
                return NotFound();
            }

            return View(userBird);
        }

        // GET: UserBirds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserBirds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,User,BirdId,Bird")] UserBird userBird)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userBird);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userBird);
        }

        // GET: UserBirds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBird = await _context.UserBird.FindAsync(id);
            if (userBird == null)
            {
                return NotFound();
            }
            return View(userBird);
        }

        // POST: UserBirds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,User,BirdId,Bird")] UserBird userBird)
        {
            if (id != userBird.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userBird);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserBirdExists(userBird.Id))
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
            return View(userBird);
        }

        // GET: UserBirds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBird = await _context.UserBird
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userBird == null)
            {
                return NotFound();
            }

            return View(userBird);
        }

        // POST: UserBirds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userBird = await _context.UserBird.FindAsync(id);
            if (userBird != null)
            {
                _context.UserBird.Remove(userBird);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserBirdExists(int id)
        {
            return _context.UserBird.Any(e => e.Id == id);
        }
    }
}
