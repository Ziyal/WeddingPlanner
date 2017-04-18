using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class WeddingViewModel : BaseEntity
    {
        [Required]
        public int WeddingId { get; set; }
        
        [Required]
        public string Bride { get; set; }

        [Required]
        public string Groom { get; set; }

        [Required]
        [InTheFuture]
        public DateTime Date { get; set; }

        [Required]
        public string Address { get; set; }


        
    }
}