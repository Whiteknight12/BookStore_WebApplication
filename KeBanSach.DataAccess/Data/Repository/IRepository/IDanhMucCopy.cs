using KeBanSach.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeBanSach.DataAccess.Data.Repository.IRepository
{
    public interface IDanhMucCopy: IRepository<DanhMucCopy>
    {
        public void Add(DanhMucCopy item);
        public void Update(DanhMucCopy item);
    }
}
