using KeBanSach.DataAccess.Data.Repository.IRepository;
using KeBanSach.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeBanSach.DataAccess.Data.Repository
{
    public class DaMuaRepository : Repository<DaMua>, IDaMua
    {
        private ApplicationDbContext _db;
        public DaMuaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Add(DaMua obj)
        {
            _db.Add(obj);
        }

        public void Update(DaMua obj)
        {
            _db.Update(obj);
        }
    }
}
