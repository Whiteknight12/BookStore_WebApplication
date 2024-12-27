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
    public class DanhMucCopyRepository : Repository<DanhMucCopy>, IDanhMucCopy
    {
        private ApplicationDbContext _db;
        private DbSet<DanhMucCopy> _dbSet;
        public DanhMucCopyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
            _dbSet=_db.Set<DanhMucCopy>();
        }

        public void Add(DanhMucCopy item)
        {
            _dbSet.Add(item);
        }

        public void Update(DanhMucCopy item)
        {
            _dbSet.Update(item);
        }
    }
}
