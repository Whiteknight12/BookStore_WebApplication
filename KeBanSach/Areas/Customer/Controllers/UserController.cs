using DocumentFormat.OpenXml.Spreadsheet;
using KeBanSach.DataAccess.Data;
using KeBanSach.DataAccess.Data.Repository.IRepository;
using KeBanSach.Models;
using KeBanSach.Models.Models;
using KeBanSach.Models.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.Identity.Client;

namespace KeBanSach.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {
        private ISanPham _sanpham;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;
        private ICart _cart;
        private IBoughtCart _boughtCart;
        private IDaMua _damua;
        private IDanhGia _danhgia;
        private ISellCanvas _sellcanvas;
        private IDanhMuc _danhmuc;
        private ISanPhamVaDanhMuc _spvdm;
        public UserController(ISanPham sanPham, 
            UserManager<IdentityUser> userManager, 
            ApplicationDbContext db, 
            ICart cart, 
            IBoughtCart boughtCart, 
            IDaMua damua, 
            IDanhGia danhgia, 
            ISellCanvas sellcanvas,
            IDanhMuc danhMuc,
            ISanPhamVaDanhMuc spvdm) 
        { 
            _sanpham = sanPham;
            _userManager = userManager;
            _db = db;
            _cart = cart;
            _boughtCart = boughtCart;
            _damua = damua;
            _danhgia = danhgia;
            _sellcanvas = sellcanvas;
            _danhmuc=danhMuc;
            _spvdm = spvdm;
        }
        [Authorize(Roles ="Admin, Customer")]
        public async Task<IActionResult> CartIndex()
        {
            var user = await _userManager.GetUserAsync(User);
            var listsanpham = _cart.GetList(u => u.UserId == user.Id);
            List<CartVM> cartvm = new List<CartVM>();
            foreach (var sanpham in listsanpham)
            {
                CartVM obj= new CartVM();
                obj.sanpham = _sanpham.Get(u => u.SanPhamId == sanpham.SanPhamId);
                obj.Number = _cart.Get(u => (u.SanPhamId == sanpham.SanPhamId && u.UserId==user.Id)).Number;
                cartvm.Add(obj);
            }
            if (_cart.GetFirstConfirmed(u => u.UserId == user.Id)!=null)
            {
                bool confirmed = (bool)_cart.GetFirstConfirmed(u => u.UserId == user.Id);
                ViewBag.Confirm = confirmed;
            }
            return View(cartvm);
        }
        public async Task<IActionResult> AddCart(int? id)
        {
            if (User.IsInRole("Customer") || User.IsInRole("Admin"))
            {
                var user=await _userManager.GetUserAsync(User);
                if (user!=null)
                {
                    var true_user = (CustomedUser)user;
                    if (string.IsNullOrEmpty(true_user.PhoneNumber) || string.IsNullOrEmpty(true_user.Address) || string.IsNullOrEmpty(true_user.Name))
                    {
                        TempData["error"] = "Vui lòng nhập đầy đủ thông tin trước khi thêm vào giỏ hàng!";
                        return RedirectToAction("Detail", "Home", new { sanphamid = id });
                    }
                    else
                    {
                        Cart cart = new Cart();
                        cart.SanPhamId = id;
                        cart.UserId = user.Id;
                        cart.Number = 1;
                        cart.Confirmed = false;
                        _cart.Add(cart);
                        _cart.Save();
                        TempData["success"] = "Đã Thêm Sản Phẩm Vào Giỏ Hàng Thành Công";
                        return RedirectToAction("Detail", "Home", new { sanphamid = id });
                    }
                }
            }
            TempData["error"] = "Phải Đăng Nhập Mới Có Thể Truy Cập Giỏ Hàng";
            return RedirectToAction("Detail", "Home", new { sanphamid = id });
        }
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Remove(int? sanphamid)
        {
            var user=await _userManager.GetUserAsync(User);
            if (sanphamid!=null)
            {
                var cart=_cart.Get(u=>(u.SanPhamId==sanphamid && u.UserId==user.Id));
                _cart.Delete(cart);
                _cart.Save();
                TempData["success"] = "Bỏ chọn sản phẩm thành công!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = "Không tìm thấy sản phẩm để loại bỏ";
                return RedirectToAction("CartIndex");
            }
        }
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Received()
        {
            var user=await _userManager.GetUserAsync(User);
            var cart = _cart.GetList(u => u.UserId == user.Id);
            Random random = new Random();
            var allboughtcart=_boughtCart.GetAll();
            int randomnumber = random.Next(1, int.MaxValue);
            foreach (var boughtcart in allboughtcart)
            {
                if (boughtcart.Code!=null)
                {
                    if (randomnumber == boughtcart.Code) randomnumber = random.Next(1, int.MaxValue);
                }
            }
            foreach (var item in cart)
            {
                BoughtCart boughtitem = new BoughtCart();
                boughtitem.Code = randomnumber;
                boughtitem.SanPhamId=item.SanPhamId;
                boughtitem.UserId=item.UserId;
                boughtitem.Number=item.Number;
                _boughtCart.Add(boughtitem);
                _cart.Delete(item);
            }
            _cart.Save();
            _boughtCart.Save();
            TempData["success"] = "Đơn hàng thêm thành công";
            return RedirectToAction("CartIndex");
        }
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> BoughtCartIndex()
        {
            var user = await _userManager.GetUserAsync(User);
            var listsanpham = _boughtCart.GetList(u => u.UserId == user.Id);
            var groupedsanpham = listsanpham.GroupBy(u => u.Code);
            List<BoughtCartVM> boughtcartvm = new List<BoughtCartVM>();
            foreach (var group in groupedsanpham)
            {
                BoughtCartVM obj = new BoughtCartVM();
                obj.items = new Dictionary<SanPham, int>();
                foreach(var item in group)
                {
                    var sanpham=_sanpham.Get(u=>u.SanPhamId==item.SanPhamId);
                    obj.items[sanpham] = item.Number.Value;
                }
                obj.Delivered = group.FirstOrDefault().Delivered;
                boughtcartvm.Add(obj);
            }
            return View(boughtcartvm);
        }
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> DaNhanHang(string username, string CartId)
        {
            var user=await _userManager.FindByNameAsync(username);
            var numbers = CartId.Split(',').Select(int.Parse).ToList();
            if (User.IsInRole("Admin"))
            {
                foreach (var boughtcartid in numbers)
                {
                    var boughtcart=_boughtCart.Get(u=>u.CartId==boughtcartid);
                    //
                    var sanpham = _sanpham.Get(u => u.SanPhamId == boughtcart.SanPhamId);
                    if (sanpham.Number >= boughtcart.Number)
                    {
                        sanpham.Number -= boughtcart.Number ?? 0;
                        _sanpham.Update(sanpham);
                        _sanpham.Save();
                    }
                    else
                    {
                        TempData["error"] = "Không đủ sản phẩm để bán";
                        return RedirectToAction("Status", "SanPham", new { area = "Admin" });
                    }
                    //
                    boughtcart.Delivered = Utility.ExtensiveVariables.TinhTrangDonHang.dagiao;
                    _boughtCart.Save();
                    DaMua obj= new DaMua();
                    obj.SanPhamId=boughtcart.SanPhamId;
                    obj.UserId = boughtcart.UserId;
                    _damua.Add(obj);
                    _damua.Save();
                    SellCanvas sellcanvas= new SellCanvas();
                    sellcanvas.spid = boughtcart.SanPhamId ?? 0;
                    sellcanvas.Number = boughtcart.Number ?? 0;
                    sellcanvas.Time=DateTime.Now;
                    sellcanvas.dmid = _sanpham.Get(u => u.SanPhamId == boughtcart.SanPhamId).DanhMucId;
                    sellcanvas.sanphamname = _sanpham.Get(u => u.SanPhamId == boughtcart.SanPhamId).Name;
                    sellcanvas.userid = boughtcart.UserId;
                    var tmpuser=await _userManager.FindByIdAsync(boughtcart.UserId);
                    var true_user=(CustomedUser)tmpuser;
                    sellcanvas.fullname = true_user.Name;
                    sellcanvas.username = true_user.UserName;
                    _sellcanvas.Add(sellcanvas);
                    _sellcanvas.Save();
                }
                return RedirectToAction("Status", "SanPham", new { area = "Admin" });
            }
            else
            {
                foreach (var sanphamid in numbers)
                {
                    var boughtcart=_boughtCart.Get(u=>(u.SanPhamId==sanphamid && u.UserId==user.Id));
                    _boughtCart.Delete(boughtcart);
                    _boughtCart.Save();
                }
                return RedirectToAction("BoughtCartIndex");
            }
        }
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> AdminDaNhanHang(string username, string CartId)
        {
            var user = await _userManager.FindByNameAsync(username);
            var numbers = CartId.Split(',').Select(int.Parse).ToList();
            foreach (var sanphamid in numbers)
            {
                var boughtcart = _boughtCart.Get(u => (u.SanPhamId == sanphamid && u.UserId == user.Id));
                _boughtCart.Delete(boughtcart);
                _boughtCart.Save();
            }
            return RedirectToAction("BoughtCartIndex");
        }
        [Authorize(Roles = "Admin, Customer")]
        public IActionResult DeleteComment(int DanhGiaId, int SanPhamId)
        {
            var danhgia=_danhgia.Get(u=>u.DanhGiaId==DanhGiaId);
            _danhgia.Delete(danhgia);
            _danhgia.Save();
            TempData["success"] = "Xóa bình luận thành công!";
            return RedirectToAction("Detail", "Home", new { sanphamid = SanPhamId });
        }
        [Authorize(Roles = "Admin, Customer")]
        public IActionResult EditComment(int DanhGiaId, int SanPhamId)
        {
            var danhgia = _danhgia.Get(u => u.DanhGiaId == DanhGiaId);
            ViewBag.SanPhamId=SanPhamId;
            return View(danhgia);
        }
        [HttpPost]
        public IActionResult EditComment(DanhGia danhgia, int SanPhamId)
        {
            _danhgia.Update(danhgia);
            _danhgia.Save();
            TempData["success"] = "Sửa đánh giá thành công!";
            return RedirectToAction("Detail", "Home", new {sanphamid= SanPhamId});
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> XoaDonHang(string username, string CartId)
        {
            var user = await _userManager.FindByNameAsync(username);
            var numbers = CartId.Split(',').Select(int.Parse).ToList();
            foreach (var number in numbers)
            {
                var boughtcart = _boughtCart.Get(u => u.CartId == number);
                boughtcart.Delivered = Utility.ExtensiveVariables.TinhTrangDonHang.bihuy;
                _boughtCart.Update(boughtcart);
            }
            _boughtCart.Save();
            TempData[("success")] = "Hủy đơn hàng thành công";
            return RedirectToAction("Status", "SanPham", new {area="Admin"});
        }
    }
}
