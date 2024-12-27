using KeBanSach.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeBanSach.DataAccess.Data.Repository.IRepository
{
    public interface ISellCanvas: IRepository<SellCanvas>
    {
        public void Add(SellCanvas item);
        public void Update(SellCanvas item);
    }
}
