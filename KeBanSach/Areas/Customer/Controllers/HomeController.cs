using KeBanSach.DataAccess.Data.Repository.IRepository;
using KeBanSach.Models;
using KeBanSach.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Security.Claims;
using System.Globalization;

namespace KeBanSach.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ISanPham _sanpham;
        private IDanhMuc _danhmuc;
        private SignInManager<IdentityUser> _signinManager;
        private IDaMua _damua;
        private IDanhGia _danhGia;
        private UserManager<IdentityUser> _userManager;
        private ISanPhamVaDanhMuc _spvdm;
        private ISellCanvas _sellcanvas;
        public HomeController(ILogger<HomeController> logger, 
            ISanPham sanPham, 
            IDanhMuc danhMuc, 
            SignInManager<IdentityUser> signInManager,
            IDaMua daMua,
            IDanhGia danhGia,
            UserManager<IdentityUser> userManager,
            ISanPhamVaDanhMuc sanPhamVaDanhMuc,
            ISellCanvas sellCanvas)
        {
            _logger = logger;
            _sanpham = sanPham;
            _danhmuc = danhMuc;
            _signinManager = signInManager;
            _damua = daMua;
            _danhGia = danhGia;
            _userManager = userManager;
            _spvdm = sanPhamVaDanhMuc;
            _sellcanvas = sellCanvas;
        }

        public IActionResult Index()
        {
            var bestseller = new Dictionary<SanPham, int>();
            var list = _sellcanvas.GetAll();
            var listsanpham = _sanpham.GetAll();
            foreach (var sanpham in listsanpham)
            {
                int sold = 0;
                var listsellcanvas = list.Where(u => u.spid == sanpham.SanPhamId);
                foreach (var item in listsellcanvas) sold += item.Number;
                bestseller[sanpham] = sold;
            }
            ViewBag.BestSeller = bestseller.OrderByDescending(pair => pair.Value).Take(4).ToList();
            ViewBag.Filter = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Tháng này",
                    Value = "ThisMonth"
                },
                new SelectListItem
                {
                    Text = "Tuần này",
                    Value = "ThisWeek"
                },
                new SelectListItem
                {
                    Text = "Hôm nay",
                    Value = "Today"
                }
            },
            "Value",
            "Text");
            return View(_sanpham.GetAll().ToList());
        }
        [HttpPost]
        public IActionResult Index(string? searchString, string? BestSellerFilter)
        {
            if (!string.IsNullOrEmpty(BestSellerFilter))
            {
                if (BestSellerFilter=="ThisMonth")
                {
                    var bestseller = new Dictionary<SanPham, int>();
                    var list = _sellcanvas.GetAll();
                    var listtmpsanpham = _sanpham.GetAll();
                    foreach (var sanpham in listtmpsanpham)
                    {
                        int sold = 0;
                        var listsellcanvas = list.Where(u => u.spid == sanpham.SanPhamId && u.Time.Month==DateTime.Now.Month);
                        if (listsellcanvas.Count() <= 0) continue;
                        else
                        {
                            foreach (var item in listsellcanvas) sold += item.Number;
                            bestseller[sanpham] = sold;
                        }
                    }
                    ViewBag.BestSeller = bestseller.OrderByDescending(pair => pair.Value).Take(4).ToList();
                }
                else if (BestSellerFilter=="ThisWeek")
                {
                    var bestseller = new Dictionary<SanPham, int>();
                    var list = _sellcanvas.GetAll();
                    var listtmpsanpham = _sanpham.GetAll();
                    var currentWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                        DateTime.Now,
                        CalendarWeekRule.FirstDay,
                        DayOfWeek.Monday 
                    );
                    foreach (var sanpham in listtmpsanpham)
                    {
                        int sold = 0;
                        var listsellcanvas = list.Where(u => u.spid == sanpham.SanPhamId && CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                            u.Time,
                            CalendarWeekRule.FirstDay,
                            DayOfWeek.Monday
                        ) == currentWeek);
                        if (listsellcanvas.Count() <= 0) continue;
                        else
                        {
                            foreach (var item in listsellcanvas) sold += item.Number;
                            bestseller[sanpham] = sold;
                        }
                    }
                    ViewBag.BestSeller = bestseller.OrderByDescending(pair => pair.Value).Take(4).ToList();
                }
                else if (BestSellerFilter=="Today")
                {
                    var bestseller = new Dictionary<SanPham, int>();
                    var list = _sellcanvas.GetAll();
                    var listtmpsanpham = _sanpham.GetAll();
                    foreach (var sanpham in listtmpsanpham)
                    {
                        int sold = 0;
                        var listsellcanvas = list.Where(u => u.spid == sanpham.SanPhamId && u.Time.Date == DateTime.Now.Date);
                        if (listsellcanvas.Count() <= 0) continue;
                        else
                        {
                            foreach (var item in listsellcanvas) sold += item.Number;
                            bestseller[sanpham] = sold;
                        }
                    }
                    ViewBag.BestSeller = bestseller.OrderByDescending(pair => pair.Value).Take(4).ToList();
                }
                else
                {
                    var bestseller = new Dictionary<SanPham, int>();
                    var list = _sellcanvas.GetAll();
                    var listtmpsanpham = _sanpham.GetAll();
                    foreach (var sanpham in listtmpsanpham)
                    {
                        int sold = 0;
                        var listsellcanvas = list.Where(u => u.spid == sanpham.SanPhamId);
                        foreach (var item in listsellcanvas) sold += item.Number;
                        bestseller[sanpham] = sold;
                    }
                    ViewBag.BestSeller = bestseller.OrderByDescending(pair => pair.Value).Take(4).ToList();
                }
            }
            else
            {
                var bestseller = new Dictionary<SanPham, int>();
                var list = _sellcanvas.GetAll();
                var listtmpsanpham = _sanpham.GetAll();
                foreach (var sanpham in listtmpsanpham)
                {
                    int sold = 0;
                    var listsellcanvas = list.Where(u => u.spid == sanpham.SanPhamId);
                    foreach (var item in listsellcanvas) sold += item.Number;
                    bestseller[sanpham] = sold;
                }
                ViewBag.BestSeller = bestseller.OrderByDescending(pair => pair.Value).Take(4).ToList();
            }
            ViewBag.Filter = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Tháng này",
                    Value = "ThisMonth"
                },
                new SelectListItem
                {
                    Text = "Tuần này",
                    Value = "ThisWeek"
                },
                new SelectListItem
                {
                    Text = "Hôm nay",
                    Value = "Today"
                }
            },
            "Value",
            "Text");
            if (!string.IsNullOrEmpty(searchString))
            {
                var listsanpham = _sanpham.GetList(u => u.Name.Contains(searchString)).ToList();
                TempData["searchsuccess"] = $"Hiển thị kết quả tìm kiếm cho {searchString}";
                return View(listsanpham);
            }
            return View(_sanpham.GetAll().ToList());
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> Detail(int? sanphamid)
        {
            SanPham obj = _sanpham.Get(u => u.SanPhamId == sanphamid);
            obj.DanhMuc = _danhmuc.Get(u => u.DanhMucId == obj.DanhMucId);
            var listspvdm=_spvdm.GetList(u=>u.spid==obj.SanPhamId);
            foreach (var spvdm in listspvdm)
            {
                obj.ListDanhMuc.Add(_danhmuc.Get(u=>u.DanhMucId==spvdm.dmid));  
            }
            bool HaveBought = false;
            if (_signinManager.IsSignedIn(User))
            {
                var user=await _userManager.GetUserAsync(User);
                var damualist = _damua.GetList(u => u.UserId == user.Id);
                foreach (var entity in damualist)
                {
                    if (entity.SanPhamId == sanphamid) HaveBought = true;
                }
                var true_user=(CustomedUser)user;
                ViewBag.UserAvatar = true_user.UserImgUrl;
                ViewBag.UserId=true_user.Id;
            }
            ViewBag.HaveBought= HaveBought;
            List<(CustomedUser Key, DanhGia Value)> Comments = new List<(CustomedUser Key, DanhGia Value)>();
            var listdanhgia = _danhGia.GetList(u => u.SanPhamId == sanphamid);
            foreach (var danhgia in listdanhgia)
            {
                var user = await _userManager.FindByIdAsync(danhgia.UserId);
                var true_user = (CustomedUser)user;
                Comments.Add((true_user, danhgia));
            }
            ViewBag.Comments=Comments;
            int amount = 0;
            foreach (var sellcanvas in _sellcanvas.GetList(u => u.spid == sanphamid)) amount += sellcanvas.Number;
            ViewBag.amount=amount;
            return View(obj);
        }
        [HttpPost]
        public IActionResult Detail(int SanPhamId, string UserId, string DanhGia)
        {
            DanhGia danhgia=new DanhGia();
            danhgia.UserId=UserId;
            danhgia.SanPhamId=SanPhamId;
            danhgia.NoiDung = DanhGia;
            _danhGia.Add(danhgia);
            _danhGia.Save();
            TempData[("success")] = "Đăng bình luận thành công!";
            int amount = 0;
            foreach (var sellcanvas in _sellcanvas.GetList(u => u.spid == SanPhamId)) amount += sellcanvas.Number;
            ViewBag.amount = amount;
            return RedirectToAction("Detail", new { sanphamid = SanPhamId });
        }
        public IActionResult All()
        {
            return View(_sanpham.GetAll().ToList());
        }
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Comment(int SanPhamId, ClaimsPrincipal User)
        {
            var user = await _userManager.GetUserAsync(User);
            DanhGia danhgia= new DanhGia();
            danhgia.SanPhamId = SanPhamId;
            danhgia.UserId = user.Id;
            _danhGia.Add(danhgia);
            _danhGia.Save();
            return RedirectToAction("Detail", SanPhamId);
        }
        public IActionResult CategoryFilter(int DanhMucId)
        {
            var listsanpham=new List<SanPham>();
            foreach (var spvdm in _spvdm.GetList(u=>u.dmid==DanhMucId)) listsanpham.Add(_sanpham.Get(u=>u.SanPhamId==spvdm.spid));
            var danhmuc=_danhmuc.Get(u=>u.DanhMucId==DanhMucId);
            ViewBag.danhmuc = danhmuc;  
            return View(listsanpham);
        }
    }
}
