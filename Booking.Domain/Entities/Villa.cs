using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{

    public class Villa
    {
        [Key]
        public int Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        [DisplayName("Per one night")]
        public double Price { get; set; }

        [DisplayName("Square")]
        public int Sqrt { get; set; }

        public int Occupancy { get; set; }

        public IFormFile? Image { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime? UpdateDate { get; set; }
        public DateTime? CreateDate { get; set; }


    }
}
