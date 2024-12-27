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
    public class SellCanvasRepository : Repository<SellCanvas>, ISellCanvas
    {
        private ApplicationDbContext _db;
        private DbSet<SellCanvas> _dbSet;
        public SellCanvasRepository(ApplicationDbContext db) : base(db)
        {
            _db= db;
            _dbSet=_db.Set<SellCanvas>();
        }

        public void Add(SellCanvas item)
        {
            _dbSet.Add(item);   
        }

        public void Update(SellCanvas item)
        {
            _dbSet.Update(item);
        }
    }
}
