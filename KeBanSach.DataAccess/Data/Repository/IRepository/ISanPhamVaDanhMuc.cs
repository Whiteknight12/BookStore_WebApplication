using KeBanSach.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeBanSach.DataAccess.Data.Repository.IRepository
{
    public interface ISanPhamVaDanhMuc: IRepository<SanPhamVaDanhMuc>
    {
        public void Add(SanPhamVaDanhMuc obj);
        public void Update(SanPhamVaDanhMuc obj);
    }
}
