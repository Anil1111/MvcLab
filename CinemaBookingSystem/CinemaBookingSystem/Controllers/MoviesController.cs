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

        public IActionResult Home()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        // GET: Movies
        public async Task<IActionResult> Index(string sortOrder)
        {
            // Convert sort order
            ViewData["NameSortParm"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["TicketSortParm"] = sortOrder == "tickets" ? "tickets_desc" : "tickets";
            ViewData["ShowTimeSortParm"] = sortOrder == "showtime" ? "showtime_desc" : "showtime";
            ViewData["TheaterSortParm"] = sortOrder == "theater" ? "theater_desc" : "theater";

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
                case "theater":
                    sortQuery = sortQuery.OrderBy(m => m.Theater.Id);
                    break;
                case "theater_desc":
                    sortQuery = sortQuery.OrderByDescending(m => m.Theater.Id);
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
        public async Task<IActionResult> Buy(int id, Movie validator)
        {
            var movie = await _context.Movies.Include(t => t.Theater).SingleOrDefaultAsync(m => m.Id == id);

            if (id != movie.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    movie.Tickets = movie.Tickets - validator.TicketValidator;

                    if (movie.Tickets >= 0)
                    {
                        movie.TicketValidator = validator.TicketValidator;
                        _context.Update(movie);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        // TEMPORARY FIX - REMOVE WHEN DONE
                        movie.Tickets = movie.Tickets + validator.TicketValidator;
                        movie.TicketErrorMessage = "Not enough tickets available, try again";
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
                return RedirectToAction(nameof(TicketConfirmationView), new { id = movie.Id, ticketsBought = movie.TicketValidator });
            }
            return View(movie);
        }

        //GET: Movie/TicketConfirmation/id
        public async Task<IActionResult> TicketConfirmationView(int? id, int ticketsBought)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.Include(t => t.Theater).SingleOrDefaultAsync(m => m.Id == id);
            movie.TicketValidator = ticketsBought;

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
