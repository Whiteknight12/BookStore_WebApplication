using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeBanSach.Models.Models.ViewModels
{
    public class SanPhamStatusVM
    {
        public string Username { get; set; }
        public string RealName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public List<BillVM> Bills { get; set; } = new List<BillVM>();
    }
}
