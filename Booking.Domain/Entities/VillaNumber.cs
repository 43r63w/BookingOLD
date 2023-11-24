using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Entities
{
    public class VillaNumber
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Villa_Number { get; set; }

        public int VillaId { get; set; }
        [ForeignKey("VillaId")]
        [ValidateNever]
        public Villa Villa { get; set; }

        public string? SpecialDetails { get; set; }

    }
}
