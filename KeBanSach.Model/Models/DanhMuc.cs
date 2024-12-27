using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeBanSach.Models
{
    public class DanhMuc
    {
        [Key]
        public int DanhMucId { get; set; }
        [Required]
        [MaxLength(20)]
        [Display(Name="Tên Danh Mục")]
        public string Name { get; set; }
        [ValidateNever]
        [NotMapped]
        public List<SanPham> ListSanPham { get; set; } = new List<SanPham>();
    }
}
