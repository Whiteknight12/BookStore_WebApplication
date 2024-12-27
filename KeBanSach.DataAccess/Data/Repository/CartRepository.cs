using KeBanSach.DataAccess.Data.Repository.IRepository;
using KeBanSach.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KeBanSach.DataAccess.Data.Repository
{
    public class CartRepository : Repository<Cart>, ICart
    {
        private ApplicationDbContext _db;
        public CartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Add(Cart obj)
        {
            var oldcart=_db.Set<Cart>().FirstOrDefault(u=>(u.UserId== obj.UserId && u.SanPhamId==obj.SanPhamId));
            if (oldcart != null)
            {
                if (oldcart.Number == null)
                {
                    oldcart.Number = 1;
                    _db.Set<Cart>().Update(oldcart);
                    _db.SaveChanges();
                }
                oldcart.Number++;
                _db.Set<Cart>().Update(oldcart);
            }
            else _db.Set<Cart>().Add(obj);
        }

        public void Update(Cart obj)
        {
            _db.Set<Cart>().Update(obj);
        }
        public bool? GetFirstConfirmed(Expression<Func<Cart, bool>> filter)
        {
            IQueryable<Cart> c = _db.Set<Cart>();
            Cart cart=c.FirstOrDefault(filter);
            if (cart == null) return null;
            return cart.Confirmed;
        }
    }
}
