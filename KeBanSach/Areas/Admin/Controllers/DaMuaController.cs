using KeBanSach.DataAccess.Data.Repository.IRepository;
using KeBanSach.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KeBanSach.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DaMuaController: Controller
    {
        private ISellCanvas _sellcanvas;
        private UserManager<IdentityUser> _usermanager;
        private ISanPham _sanpham;
        private ISellCanvas sellCanvas;
        public DaMuaController(ISellCanvas daMua, UserManager<IdentityUser> userManager, ISanPham sanPham, ISellCanvas sellCanvas)
        {
            _sellcanvas = daMua;
            _usermanager = userManager;
            _sanpham = sanPham;
            this.sellCanvas = sellCanvas;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> History()
        {
            var list=_sellcanvas.GetAll();
            foreach (var item in list)
            {
                if (item.fullname==null)
                {
                    var user = await _usermanager.FindByIdAsync(item.userid);
                    var true_user=(CustomedUser)user;
                    item.fullname = true_user.Name;
                }
                if (item.sanphamname==null)
                {
                    item.sanphamname = _sanpham.Get(u => u.SanPhamId == item.spid).Name;
                }
                _sellcanvas.Update(item);
            }
            _sellcanvas.Save();
            return View(list.OrderByDescending(u=>u.Time));
        }
        [HttpPost]
        public async Task<IActionResult> History(DateTime? startDate, DateTime? endDate, int SearchSanPhamID, string? SearchUserID)
        {
            var list = _sellcanvas.GetAll();
            if (startDate.HasValue && endDate.HasValue) list=list.Where(u=>u.Time.Date>=startDate.Value.Date && u.Time.Date<=endDate.Value.Date);
            if (SearchSanPhamID != -1) list = list.Where(u => u.spid == SearchSanPhamID);
            if (!string.IsNullOrEmpty(SearchUserID)) list=list.Where(u=>u.username.Contains(SearchUserID));
            foreach (var item in list)
            {
                if (item.fullname == null)
                {
                    var user = await _usermanager.FindByIdAsync(item.userid);
                    var true_user = (CustomedUser)user;
                    item.fullname = true_user.Name;
                }
                if (item.sanphamname == null)
                {
                    item.sanphamname = _sanpham.Get(u => u.SanPhamId == item.spid).Name;
                }
                _sellcanvas.Update(item);
            }
            _sellcanvas.Save();
            return View(list.OrderByDescending(u => u.Time));
        }
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> BoughtHistory()
        {
            var user = await _usermanager.GetUserAsync(User);
            var true_user=(CustomedUser)user;
            var boughtlist = _sellcanvas.GetList(u => u.userid == true_user.Id);
            return View(boughtlist.ToList());
        }
    }
}
