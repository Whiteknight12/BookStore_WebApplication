using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeBanSach.Models.Models
{
    public class DanhMucCopy
    {
        [Key]
        public int DanhMucCopyId { get; set; }
        [ValidateNever]
        public int dmid { get; set; }
        [ValidateNever]
        public string DanhMucName { get; set; }
    }
}
