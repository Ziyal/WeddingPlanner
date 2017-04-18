using System;
using System.Collections.Generic;

namespace WeddingPlanner.Models
{
    public class User : BaseEntity
    {

        public int UserId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime CreatedAt { get; set;}
        public DateTime UpdatedAt { get; set;}
        public List<Wedding> Weddings { get; set; }

        // Many to Many for Guests
        public List<Guest> Guests { get; set; }
        public User() {
            Guests = new List<Guest>();
            Weddings = new List<Wedding>();
        }


        
    }
}