using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KeBanSach.Utility.ExtensiveVariables;

namespace KeBanSach.Models.Models
{
    public class BoughtCart
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
        public int? Number { get; set; }
        [ValidateNever]
        public int? Code { get; set; }
        public TinhTrangDonHang Delivered { get; set; } = TinhTrangDonHang.chogiao;
    }
}
