using KeBanSach.Models;

namespace KeBanSach.DataAccess.Data.Repository.IRepository
{
    public interface ISanPham: IRepository<SanPham> 
    {
        public void Update(SanPham obj);
        public void Add(SanPham obj);
    }
}
