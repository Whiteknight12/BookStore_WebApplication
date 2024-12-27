using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace KeBanSach.Models
{
    public class SanPham
    {
        [Key]
        public int SanPhamId { get; set; }
        [Required]
        [Display(Name="Tên Sản Phẩm")]
        public string Name { get; set; }
        [Display(Name = "Tác Giả")]
        public string Author { get; set; }
        [ValidateNever]
        [Display(Name="Danh Mục")]
        public int DanhMucId { get; set; }
        [ValidateNever]
        [ForeignKey("DanhMucId")]
        public DanhMuc DanhMuc {  get; set; }
        [Display(Name="Ảnh Sản Phẩm")]
        [ValidateNever]
        public string AnhSanPham { get; set; }
        [Range(1, 100)]
        [Display(Name="Giá Sản Phẩm (Tính Bằng USD)")]
        public float Price { get; set; }
        [Display(Name="Số lượng")]
        [Required]
        [Range(0, int.MaxValue)]
        public int Number {  get; set; }
        [ValidateNever]
        [Display(Name = "Mô Tả")]
        public string Description { get; set; }
        [ValidateNever]
        [NotMapped]
        public List<DanhMuc> ListDanhMuc { get; set; } = new List<DanhMuc>();
    }
}
