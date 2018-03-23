using CinemaBookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaBookingSystem.Data
{
    public class CinemaDbContext : DbContext
    {
        public CinemaDbContext(DbContextOptions options):base(options) {    }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Theater> Theaters { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Movie>()
                .HasOne(x => x.Theater)
                .WithMany(y => y.Movies);
        }
    }
}
