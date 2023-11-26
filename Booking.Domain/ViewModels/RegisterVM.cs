using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ViewModels
{
    public class RegisterVM
    {

        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name="Phone number")]
        public string? PhoneNumber { get; set; }


        public bool RememberMe { get; set; }

        public string? RedirectUrl { get; set; }
     
        public string? Role { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> RoleLists { get; set; }

    }
}
