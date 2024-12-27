using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeBanSach.Models.Models
{
    public class DaMua
    {
        [Key]
        public int DaMuaId { get; set; }
        [ValidateNever]
        public string? UserId { get; set; }
        [ValidateNever]
        public int? SanPhamId { get; set; }
        public bool Bought = false;
    }
}
