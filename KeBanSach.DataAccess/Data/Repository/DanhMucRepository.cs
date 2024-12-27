using KeBanSach.DataAccess.Data.Repository.IRepository;
using KeBanSach.Models;

namespace KeBanSach.DataAccess.Data.Repository
{
    public class DanhMucRepository : Repository<DanhMuc>, IDanhMuc
    {
        private ApplicationDbContext _db;
        public DanhMucRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(DanhMuc obj)
        {
            _db.BangDanhMuc.Update(obj);
        }
        public void Add(DanhMuc obj)
        {
            _db.Set<DanhMuc>().Add(obj);
        }
    }
}
