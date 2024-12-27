using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeBanSach.Models.Models.ViewModels
{
    public class BillVM
    {
        public List<int> BillID { get; set; }
        public Dictionary<SanPham, int> items {  get; set; }
    }
}
