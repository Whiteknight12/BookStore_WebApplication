using KeBanSach.DataAccess.Data.Repository.IRepository;
using KeBanSach.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeBanSach.DataAccess.Data.Repository
{
    public class BoughtCartRepository : Repository<BoughtCart>, IBoughtCart
    {
        private ApplicationDbContext _db;
        public BoughtCartRepository(ApplicationDbContext db) : base(db)
        {
            _db= db;
        }
        public void Add(BoughtCart obj)
        {
            _db.Set<BoughtCart>().Add(obj);
        }

        public void Update(BoughtCart obj)
        {
            _db.Set<BoughtCart>().Update(obj);
        }
    }
}
