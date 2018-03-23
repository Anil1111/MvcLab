using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaBookingSystem.Models
{
    public class Theater
    {
        public int Id { get; set; }
        public int TotalCapacity { get; set; }

        public IList<Movie> Movies{ get; set; }
    }
}
