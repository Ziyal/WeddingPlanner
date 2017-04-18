using System;
using System.Collections.Generic;

namespace WeddingPlanner.Models
{
    public class Wedding : BaseEntity
    {
        public int WeddingId { get; set; }
        public string Bride { get; set; }

        public string Groom { get; set; }

        public DateTime Date { get; set; }

        public string Address { get; set; }

        public DateTime CreatedAt { get; set;}
        public DateTime UpdatedAt { get; set;}
        public int UserId { get; set; }

        // Many to Many for Guests
        public List<Guest> Guests { get; set; }
        public Wedding() {
            Guests = new List<Guest>();
        }

        
    }
}