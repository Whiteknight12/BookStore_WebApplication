using KeBanSach.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeBanSach.DataAccess.Data.Repository.IRepository
{
    public interface IBoughtCart: IRepository<BoughtCart>
    {
        public void Add(BoughtCart obj);
        public void Update(BoughtCart obj);
    }
}
