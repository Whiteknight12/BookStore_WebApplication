using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace KeBanSach.Models.Models
{
    public class CustomedUser: IdentityUser 
    {
        [ValidateNever]
        public string? Address { get; set; }
        [ValidateNever]
        public string? Name { get; set; }
        [ValidateNever]
        public string? UserImgUrl { get; set; }
    }
}
