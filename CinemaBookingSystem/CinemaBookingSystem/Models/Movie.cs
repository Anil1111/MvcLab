using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaBookingSystem.Models
{
    public class Movie
    {
        public int Id { get; set; }

        //[DisplayName("Movie")]
        public string Name { get; set; }
        
        [Range(0,100,
            ErrorMessage = "Incorrect value for tickets")]
        public int Tickets { get; set; }

        [DisplayName("Time")]
        public string ShowTime { get; set; }
        
        [DisplayName("Theater")]
        public Theater Theater { get; set; }

        [NotMapped]
        [DisplayName("Tickets (1-12)")]
        [Range(1, 12,
            ErrorMessage = "Number of tickets must be between 1 - 12")]
        public int TicketValidator { get; set; }
    }
}
