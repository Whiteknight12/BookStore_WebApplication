using KeBanSach.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KeBanSach.DataAccess.Data.Repository.IRepository
{
    public interface ICart: IRepository<Cart>
    {
        public void Add(Cart obj);
        public void Update(Cart obj);
        public bool? GetFirstConfirmed(Expression<Func<Cart, bool>>filter);
    }
}
