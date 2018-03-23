using CinemaBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaBookingSystem.Data
{
    public class InitDb
    {
        public static void Initialize(CinemaDbContext context)
        {
            //context.Database.EnsureCreated();

            //Look for any Movies
            if (context.Movies.Any())
            {
                return; // DB has been seeded
            }

            var theaters = new Theater[]
            {
                new Theater { TotalCapacity = 50},
                new Theater { TotalCapacity = 100}
            };
            foreach (Theater s in theaters)
            {
                context.Theaters.Add(s);
            }
            context.SaveChanges();

            var movies = new Movie[]
            {
                new Movie { Name="Jumanji", ShowTime= "12:00", Tickets = theaters[0].TotalCapacity, Theater = context.Theaters.FirstOrDefault(x => x.Id == 1)},
                new Movie {Name="Oldboy", ShowTime= "15:00", Tickets = theaters[0].TotalCapacity, Theater = context.Theaters.FirstOrDefault(x => x.Id == 1)},
                new Movie { Name="Jumanji", ShowTime= "12:00", Tickets = theaters[1].TotalCapacity, Theater = context.Theaters.FirstOrDefault(x => x.Id == 2)},
                new Movie {Name="Oldboy", ShowTime= "15:00",  Tickets = theaters[1].TotalCapacity, Theater = context.Theaters.FirstOrDefault(x => x.Id == 2)},
                new Movie { Name="Mallrats", ShowTime= "18:00",  Tickets = theaters[0].TotalCapacity, Theater = context.Theaters.FirstOrDefault(x => x.Id == 1)},
                new Movie {Name="Indiana Jones and the Temple of Doom", ShowTime= "21:00",  Tickets = theaters[0].TotalCapacity, Theater = context.Theaters.FirstOrDefault(x => x.Id == 1)},
                new Movie { Name="Mallrats", ShowTime= "18:00",  Tickets = theaters[1].TotalCapacity, Theater = context.Theaters.FirstOrDefault(x => x.Id == 2)},
                new Movie {Name="Indiana Jones and the Temple of Doom", ShowTime= "21:00",  Tickets = theaters[1].TotalCapacity, Theater = context.Theaters.FirstOrDefault(x => x.Id == 2)}
            };
            foreach (Movie m in movies)
            {
                context.Movies.Add(m);
            }
            context.SaveChanges();
        }
    }
}
