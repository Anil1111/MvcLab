using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaBookingSystem.Data;
using CinemaBookingSystem.Models;

namespace CinemaBookingSystem.Controllers
{
    public class MoviesController : Controller
    {
        private readonly CinemaDbContext _context;

        public MoviesController(CinemaDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string sortOrder)
        {
            // Convert sort order
            ViewData["NameSortParm"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["TicketSortParm"] = sortOrder == "tickets" ? "tickets_desc" : "tickets";
            ViewData["ShowTimeSortParm"] = sortOrder == "showtime" ? "showtime_desc" : "showtime";

            var sortQuery = from m in _context.Movies.Include(t => t.Theater)
                            select m;

            switch (sortOrder)
            {
                case "name":
                    sortQuery = sortQuery.OrderBy(m => m.Name);
                    break;
                case "name_desc":
                        sortQuery = sortQuery.OrderByDescending(m => m.Name);
                    break;
                case "tickets_desc":
                    sortQuery = sortQuery.OrderByDescending(m => m.Tickets);
                    break;
                case "tickets":
                    sortQuery = sortQuery.OrderBy(m => m.Tickets);
                    break;
                case "showtime_desc":
                    sortQuery = sortQuery.OrderByDescending(m => m.ShowTime);
                    break;
                case "showtime":
                    sortQuery = sortQuery.OrderBy(m => m.ShowTime);
                    break;
                default:
                    sortQuery = sortQuery.OrderBy(m => m.ShowTime);
                    break;
            }
            return View(await sortQuery.AsNoTracking().ToListAsync());
        }

        // GET: Movies/Buy/5
        [HttpGet]
        public async Task<IActionResult> Buy(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var movie = _context.Movies.Include(t => t.Theater).SingleOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }
            return View(await movie);
        }

        // POST: Movies/Buy/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(int id, Movie movieValidator)
        {
            var movie = await _context.Movies.SingleOrDefaultAsync(m => m.Id == id);

            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    movie.Tickets = movie.Tickets - movieValidator.TicketValidator;
                    if (movie.Tickets >= 0)
                    {
                        _context.Update(movie);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        // TEMPORARY REMOVE WHEN DONE
                        movie.Tickets = movie.Tickets + movieValidator.TicketValidator;
                        return View(movie);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
            return View(movie);
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
