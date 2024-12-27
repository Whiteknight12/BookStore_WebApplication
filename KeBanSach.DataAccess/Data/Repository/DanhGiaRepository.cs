using KeBanSach.DataAccess.Data.Repository.IRepository;
using KeBanSach.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeBanSach.DataAccess.Data.Repository
{
    public class DanhGiaRepository : Repository<DanhGia>, IDanhGia
    {
        private ApplicationDbContext _db;
        public DanhGiaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Add(DanhGia obj)
        {
            _db.Add(obj);
        }

        public void Update(DanhGia obj)
        {
            _db.Update(obj);
        }
    }
}
