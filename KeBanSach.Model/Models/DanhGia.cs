using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeBanSach.Models.Models
{
    public class DanhGia
    {
        [Key]
        public int DanhGiaId { get; set; }
        [ValidateNever]
        public string? UserId { get; set; }
        [ValidateNever]
        public int? SanPhamId { get; set; }
        [ValidateNever]
        public string? NoiDung { get; set; }
    }
}
