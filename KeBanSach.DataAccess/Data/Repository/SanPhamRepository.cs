using KeBanSach.DataAccess.Data.Repository.IRepository;
using KeBanSach.Models;
using System.ComponentModel;

namespace KeBanSach.DataAccess.Data.Repository
{
    public class SanPhamRepository : Repository<SanPham>, ISanPham
    {
        private ApplicationDbContext _db;
        public SanPhamRepository(ApplicationDbContext db) : base(db)
        {
            _db= db;
        }
        public void Update(SanPham obj)
        {
            var oldobj=_db.BangSanPham.FirstOrDefault(u=>u.SanPhamId==obj.SanPhamId);
            if (oldobj!= null)
            {
                oldobj.DanhMucId=obj.DanhMucId;
                oldobj.Author=obj.Author;
                oldobj.Description=obj.Description;
                oldobj.Name=obj.Name;
                oldobj.Price=obj.Price;
                oldobj.Number = obj.Number;
                if (obj.AnhSanPham!=null) oldobj.AnhSanPham=obj.AnhSanPham;
                _db.BangSanPham.Update(oldobj);
            }
        }
        public void Add(SanPham obj)
        {
            if (string.IsNullOrEmpty(obj.AnhSanPham)) obj.AnhSanPham = "";
            _db.Set<SanPham>().Add(obj);
        }
    }
}
