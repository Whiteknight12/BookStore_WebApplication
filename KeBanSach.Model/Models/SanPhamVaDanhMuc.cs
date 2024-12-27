using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeBanSach.Models.Models
{
    public class SanPhamVaDanhMuc
    {
        [Key]
        public int SanPhamVaDanhMucId { get; set; }
        [ValidateNever]
        public int? spid {  get; set; }
        [ValidateNever]
        public int? dmid { get; set; }
    }
}
