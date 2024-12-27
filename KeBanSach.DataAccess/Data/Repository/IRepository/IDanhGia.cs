using KeBanSach.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeBanSach.DataAccess.Data.Repository.IRepository
{
    public interface IDanhGia: IRepository<DanhGia>
    {
        public void Add(DanhGia obj);
        public void Update(DanhGia obj);
    }
}
