using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Users
    {
        private int id { get; set; }

        private string first_name { get; set; }

        private string last_name { get; set; }

        private string full_name { get; set; }

        private string job_title { get; set; }

        private string job_type { get; set; }

        private string phone { get; set; }

        private string email { get; set; }

        private string image { get; set; }

        private string country { get; set; }

        private string city { get; set; }

        private int onboarding_completion { get; set; }
    }
}
