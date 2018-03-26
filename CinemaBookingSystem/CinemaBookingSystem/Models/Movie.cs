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


        //TEMPORARY FOR TESTING
        [NotMapped]
        public string TicketErrorMessage { get; set; }
        
        public string CodeGenerator()
        {
            var code = GetLetter() + GetNumber();
            return code;
        }

        public char GetLetter()
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random rand = new Random();
            int num = rand.Next(0, chars.Length - 1);
            return chars[num];
        }
        public string GetNumber()
        {
            Random rand = new Random();
            string num = "";

            for (int i = 0; i < 5; i++)
            {
                num += rand.Next(0, 9);
            }

            return num;
        }
    }
}
