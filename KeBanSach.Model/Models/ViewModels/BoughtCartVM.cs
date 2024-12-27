using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KeBanSach.Utility.ExtensiveVariables;

namespace KeBanSach.Models.Models.ViewModels
{
    public class BoughtCartVM
    {
        public IDictionary<SanPham, int> items { get; set; }
        public TinhTrangDonHang Delivered { get; set; }
    }
}
