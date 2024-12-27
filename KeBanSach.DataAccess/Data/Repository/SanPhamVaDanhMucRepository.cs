using KeBanSach.DataAccess.Data.Repository.IRepository;
using KeBanSach.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeBanSach.DataAccess.Data.Repository
{
    public class SanPhamVaDanhMucRepository : Repository<SanPhamVaDanhMuc>, ISanPhamVaDanhMuc
    {
        private ApplicationDbContext _db;
        private DbSet<SanPhamVaDanhMuc> _dbset;
        public SanPhamVaDanhMucRepository(ApplicationDbContext db) : base(db)
        {
            _db= db;
            _dbset=db.Set<SanPhamVaDanhMuc>();
        }

        public void Add(SanPhamVaDanhMuc obj)
        {
            _dbset.Add(obj);
        }

        public void Update(SanPhamVaDanhMuc obj)
        {
            _dbset.Update(obj);
        }
    }
}
