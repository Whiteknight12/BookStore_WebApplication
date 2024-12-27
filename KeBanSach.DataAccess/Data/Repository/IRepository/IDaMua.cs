using KeBanSach.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeBanSach.DataAccess.Data.Repository.IRepository
{
    public interface IDaMua: IRepository<DaMua>
    {
        public void Add(DaMua obj);
        public void Update(DaMua obj);
    }
}
