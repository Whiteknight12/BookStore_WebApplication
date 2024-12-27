using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeBanSach.Models.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        [ValidateNever]
        public int? SanPhamId { get; set; }
        [ForeignKey("SanPhamId")]
        [ValidateNever]
        public SanPham SanPham { get; set; }
        [ValidateNever]
        public string? UserId { get; set; }
        [ValidateNever]
        [ForeignKey("UserId")]
        public CustomedUser User { get; set; }
        [ValidateNever]
        public int? Number {  get; set; }
        public bool? Confirmed {  get; set; }
    }
}
