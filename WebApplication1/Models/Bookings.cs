using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Bookings
    {
        public int id { get; set; }

        public int user_id { get; set; }
    
        public string starts_at { get; set; }

        public string booked_at { get; set; }

        public int booked_for { get; set; }

        public int apartment_id { get; set; }
        
        public int confirmed { get; set; }
    }
}
