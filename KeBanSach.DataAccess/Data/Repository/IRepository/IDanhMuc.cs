using KeBanSach.Models;

namespace KeBanSach.DataAccess.Data.Repository.IRepository
{
    public interface IDanhMuc: IRepository<DanhMuc>
    {
        public void Update(DanhMuc obj);
        public void Add(DanhMuc obj);
    }
}
