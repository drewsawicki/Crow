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
using Crow.ViewModels;

namespace Crow.Controllers
{
    public class PhotosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PhotosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Photos
        public async Task<IActionResult> Index(int Id)
        {

            var userBird = await _context.UserBird
                .Include(ub => ub.Bird)
                .FirstOrDefaultAsync(ub => ub.Id == Id);

            if (userBird == null)
                return NotFound();

            var photos = await _context.Photo
                .Where(p => p.UserBirdId == Id)
                .Include(p => p.UserBird.Bird)
                .ToListAsync();


            ViewBag.UserBirdId = Id;
            ViewBag.BirdCommonName = userBird.Bird.CommonName;


            return View(photos);
        }

        [HttpGet]
        public IActionResult DisplayPhoto(int id)
        {
            var photo = _context.Photo.Find(id);
            if (photo == null)
                return NotFound();

            var fileBytes = System.IO.File.ReadAllBytes(photo.FilePath);
            return File(fileBytes, "image/jpeg");
        }

        // GET: Photos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photo.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }

            ViewBag.UserBirdId = photo.UserBirdId;

            return View(photo);
        }

        // GET: Photos/Create
        public IActionResult Create(int userBirdId)
        {
            var viewModel = new PhotoUploadViewModel
            {
                Photo = new Photo
                {
                    UserBirdId = userBirdId
                }
            };

            ViewBag.UserBirdId = userBirdId;
            return View(viewModel);
        }

        // POST: Photos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(PhotoUploadViewModel model)
        {

            var userBird = await _context.UserBird
                .Include(ub => ub.Bird)
                .FirstOrDefaultAsync(ub => ub.Id == model.Photo.UserBirdId);

            if (userBird == null)
            {
                return NotFound();
            }

            if (model.File != null && model.File.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(fileStream);
                }

                // Create and save the Photo entity
                var photo = new Photo
                {
                    FileName = fileName,
                    FilePath = filePath,
                    Location = model.Photo.Location,
                    Date = model.Photo.Date,
                    Time = model.Photo.Time,
                    Notes = model.Photo.Notes,
                    UserBirdId = model.Photo.UserBirdId
                };

                userBird.Photos.Add(photo);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", new { Id = model.Photo.UserBirdId });
        }


        // GET: Photos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photo.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }

            ViewBag.UserBirdId = photo.UserBirdId;

            return View(photo);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Location,Date,Time,Notes")] Photo photo)
        {
            if (id != photo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(photo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotoExists(photo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { Id = photo.UserBirdId });
            }
            return View(photo);
        }

        // GET: Photos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            ViewBag.UserBirdId = photo.UserBirdId;
            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photo = await _context.Photo.FindAsync(id);
            if (photo != null)
            {
                _context.Photo.Remove(photo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { Id = photo.UserBirdId });
        }

        private bool PhotoExists(int id)
        {
            return _context.Photo.Any(e => e.Id == id);
        }
    }
}
