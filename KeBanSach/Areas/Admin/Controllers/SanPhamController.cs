using ClosedXML.Excel;
using ExcelDataReader;
using KeBanSach.DataAccess.Data.Repository.IRepository;
using KeBanSach.Models;
using KeBanSach.Models.Models;
using KeBanSach.Models.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Data;
using System.Reflection.Metadata;
using System.Text;

namespace KeBanSach.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class SanPhamController : Controller
    {
        private ISanPham _sanpham;
        private IDanhMuc _danhmuc;
        private ISellCanvas _sellcanvas;
        private IWebHostEnvironment _webHostEnvironment;
        private List<SelectListItem> listdanhmuc;
        private UserManager<IdentityUser> _userManager;
        private ICart _cart;
        private IBoughtCart _boughtcart;
        private ISanPhamVaDanhMuc _spvdm;
        string wwwroot;
        public SanPhamController(IDanhMuc danhmuc, 
            ISanPham sanpham, 
            IWebHostEnvironment webHostEnvironment, 
            UserManager<IdentityUser> userManager, 
            ICart cart, 
            IBoughtCart boughtcart, 
            ISellCanvas sellCanvas,
            ISanPhamVaDanhMuc spvdm)
        {
            _danhmuc = danhmuc;
            _sanpham = sanpham;
            listdanhmuc = _danhmuc.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.DanhMucId.ToString()
            }).ToList();
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _cart = cart;
            _boughtcart = boughtcart;
            _sellcanvas = sellCanvas;
            _spvdm = spvdm;
        }
        public IActionResult Index()
        {
            ViewBag.DanhMuc = listdanhmuc;
            List<SanPham> listsanpham = new List<SanPham>();
            foreach (var spvdm in _spvdm.GetAll())
            {
                var sp = _sanpham.Get(u => u.SanPhamId == spvdm.spid);
                if (sp != null)
                {
                    sp.ListDanhMuc.Add(_danhmuc.Get(u => u.DanhMucId == spvdm.dmid));
                    listsanpham.Add(sp);
                }
            }
            return View(listsanpham.DistinctBy(u=>u.SanPhamId).ToList());
        }
        [HttpPost]
        public IActionResult Index(string SearchCategory, string searchString, List<int> DeleteSanPhamId)
        {
            if (DeleteSanPhamId.Count > 0)
            {
                foreach (var id in DeleteSanPhamId)
                {
                    SanPham deleteobj = _sanpham.Get(u => u.SanPhamId == id);
                    var listcart = _cart.GetList(u => u.SanPhamId == deleteobj.SanPhamId);
                    var listboughtcart = _boughtcart.GetList(u => u.SanPhamId == deleteobj.SanPhamId);
                    if (listcart.Count() > 0 || listboughtcart.Count() > 0)
                    {
                        TempData["error"] = "Không thể xóa vì sản phẩm đang được đặt mua";
                        return RedirectToAction("Index");
                    }
                    //Xoa anh
                    wwwroot = _webHostEnvironment.WebRootPath;
                    if (deleteobj != null && !string.IsNullOrEmpty(deleteobj.AnhSanPham))
                    {
                        var oldpath = Path.Combine(wwwroot, deleteobj.AnhSanPham.TrimStart('\\'));
                        if (System.IO.File.Exists(oldpath)) System.IO.File.Delete(oldpath);
                    }
                    _sanpham.Delete(deleteobj);
                    foreach (var spvdm in _spvdm.GetList(u => u.spid == deleteobj.SanPhamId))
                    {
                        _spvdm.Delete(spvdm);
                    }
                }
                _sanpham.Save();
                TempData["success"] = "Sản Phẩm Đã Được Xóa";
            }
            var sanphamlist = _sanpham.GetAll();
            if (SearchCategory != "NoFilter")
            {
                var ls = _spvdm.GetList(u => u.dmid == int.Parse(SearchCategory));
                var newsanphamlist = new List<SanPham>();
                foreach (var spvdm in ls) newsanphamlist.Add(_sanpham.Get(u => u.SanPhamId == spvdm.spid));
                sanphamlist = newsanphamlist;
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                sanphamlist = sanphamlist.Where(u => u.Name.ToLower().Contains(searchString.ToLower()));
            }
            ViewBag.DanhMuc = listdanhmuc;
            foreach(var sanpham in sanphamlist)
            {
                var spvdmlist = _spvdm.GetList(u => u.spid == sanpham.SanPhamId);
                foreach (var spvdm in spvdmlist) sanpham.ListDanhMuc.Add(_danhmuc.Get(u => u.DanhMucId == spvdm.dmid));
            }
            return View(sanphamlist.DistinctBy(u => u.SanPhamId).ToList());
        }
        public IActionResult Create()
        {
            ViewBag.ListDanhMuc = listdanhmuc;
            return View();
        }
        [HttpPost]
        public IActionResult Create(SanPham obj, IFormFile? file, List<int> ListDanhMuc)
        {
            if (ModelState.IsValid)
            {
                //Xu ly anh 
                wwwroot = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(file.FileName) + Path.GetExtension(file.FileName);
                    string filepath = Path.Combine(wwwroot, @"images\");
                    using (var filestream = new FileStream(Path.Combine(filepath, filename), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    obj.AnhSanPham = @"\images\" + filename;
                }
                else obj.AnhSanPham = "";
                obj.DanhMucId = ListDanhMuc.FirstOrDefault();
                _sanpham.Add(obj);
                _sanpham.Save();
                foreach (var danhmucid in ListDanhMuc)
                {
                    SanPhamVaDanhMuc entity = new SanPhamVaDanhMuc();
                    entity.spid = obj.SanPhamId;
                    entity.dmid = danhmucid;
                    _spvdm.Add(entity);
                }
                _spvdm.Save();
                TempData["success"] = "Sản Phẩm Mới Đã Được Tạo";
                return RedirectToAction("Index");
            }
            ViewBag.ListDanhMuc = listdanhmuc;
            return View();
        }
        public IActionResult Edit(int? id)
        {
            SanPham obj = _sanpham.Get(sp => sp.SanPhamId == id);
            var listspvdm = _spvdm.GetList(u => u.spid == id);
            var selecteddanhmuc = new List<int>();
            foreach (var spvdm in listspvdm)
            {
                selecteddanhmuc.Add(spvdm.dmid ?? 0);
            }
            ViewBag.ListDanhMuc = _danhmuc.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.DanhMucId.ToString(),
                Selected = selecteddanhmuc.Contains(u.DanhMucId)
            }).ToList();
            return View(obj);
        }
        [HttpPost]
        public IActionResult Edit(SanPham obj, IFormFile? file, List<int> ListDanhMuc)
        {
            if (ModelState.IsValid)
            {
                //xu ly anh 
                if (file != null)
                {
                    wwwroot = _webHostEnvironment.WebRootPath;
                    string filename = Path.GetFileNameWithoutExtension(file.FileName) + Path.GetExtension(file.FileName);
                    string filepath = Path.Combine(wwwroot, @"images\");
                    if (!string.IsNullOrEmpty(obj.AnhSanPham))
                    {
                        var oldimagepath = Path.Combine(wwwroot, obj.AnhSanPham.TrimStart('\\'));
                        if (System.IO.File.Exists(oldimagepath)) System.IO.File.Delete(oldimagepath);
                    }
                    using (var filestream = new FileStream(Path.Combine(filepath, filename), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    obj.AnhSanPham = @"\images\" + filename;
                }
                var spvdm = _spvdm.GetList(u => u.spid == obj.SanPhamId);
                foreach (var entity in spvdm)
                {
                    _spvdm.Delete(entity);
                }
                obj.DanhMucId = ListDanhMuc.LastOrDefault();
                _sanpham.Update(obj);
                _sanpham.Save();
                foreach (var danhmucid in ListDanhMuc)
                {
                    SanPhamVaDanhMuc entity = new SanPhamVaDanhMuc();
                    entity.spid = obj.SanPhamId;
                    entity.dmid = danhmucid;
                    _spvdm.Add(entity);
                }
                _spvdm.Save();
                TempData["success"] = "Sản Phẩm Đã Được Sửa";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            SanPham obj = _sanpham.Get(sp => sp.SanPhamId == id);
            foreach (var spvdm in _spvdm.GetList(u=>u.spid == obj.SanPhamId))
            {
                obj.ListDanhMuc.Add(_danhmuc.Get(u => u.DanhMucId == spvdm.dmid));
            }
            ViewBag.ListDanhMuc = listdanhmuc;
            return View(obj);
        }
        [HttpPost]
        public IActionResult Delete(SanPham obj)
        {
            SanPham deleteobj = _sanpham.Get(u => u.SanPhamId == obj.SanPhamId);
            var listcart = _cart.GetList(u => u.SanPhamId == deleteobj.SanPhamId);
            var listboughtcart = _boughtcart.GetList(u => u.SanPhamId == deleteobj.SanPhamId);
            if (listcart.Count()>0 || listboughtcart.Count()>0)
            {
                TempData["error"] = "Không thể xóa vì sản phẩm đang được đặt mua";
                return RedirectToAction("Index");
            }
            else
            {
                //Xoa anh
                wwwroot = _webHostEnvironment.WebRootPath;
                if (!string.IsNullOrEmpty(deleteobj.AnhSanPham))
                {
                    var oldpath = Path.Combine(wwwroot, deleteobj.AnhSanPham.TrimStart('\\'));
                    if (System.IO.File.Exists(oldpath)) System.IO.File.Delete(oldpath);
                }
                foreach (var spvdm in _spvdm.GetList(u => u.spid == obj.SanPhamId))
                {
                    _spvdm.Delete(spvdm);
                }
                _sanpham.Delete(deleteobj);
                _sanpham.Save();
                TempData["success"] = "Sản Phẩm Đã Được Xóa";
                return RedirectToAction("Index");
            }
        }
        public async Task<IActionResult> Status()
        {
            ViewBag.SellCanvas = _sellcanvas.GetAll();
            List<SanPhamStatusVM> model = new List<SanPhamStatusVM>();
            List<BoughtCart> cartnotdelivered = _boughtcart.GetList(u => u.Delivered == Utility.ExtensiveVariables.TinhTrangDonHang.chogiao).ToList();
            var groupedcart = cartnotdelivered.GroupBy(u => u.UserId);
            if (cartnotdelivered.Count < 1)
            {
                return View(null);
            }
            else
            {
                foreach (var group in groupedcart)
                {
                    var user = await _userManager.FindByIdAsync(group.FirstOrDefault().UserId);
                    var true_user = (CustomedUser)user;
                    SanPhamStatusVM itemneedtobeadded = new SanPhamStatusVM
                    {
                        Username = true_user.UserName,
                        RealName = true_user.Name,
                        Address = true_user.Address,
                        PhoneNumber = user.PhoneNumber
                    };
                    var bills = group.GroupBy(u => u.Code);
                    foreach (var bill in bills)
                    {
                        BillVM billVM = new BillVM();
                        billVM.BillID = new List<int>();
                        billVM.items = new Dictionary<SanPham, int>();
                        foreach (var item in bill)
                        {
                            billVM.BillID.Add(item.CartId);
                            if (item.SanPhamId != null && item.Number.HasValue)
                            {
                                var sanpham = _sanpham.Get(u => u.SanPhamId == item.SanPhamId);
                                billVM.items[sanpham] = item.Number.Value;
                            }
                        }
                        itemneedtobeadded.Bills.Add(billVM);
                    }
                    model.Add(itemneedtobeadded);
                }
                return View(model);
            }
        }
        private FileResult ExportExcel(string filename, IEnumerable<SanPham> list)
        {
            DataTable dataTable = new DataTable("SanPham");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Tên Sản Phẩm"),
                new DataColumn("Tác Giả"),
                new DataColumn("Danh Mục"),
                new DataColumn("Giá Sản Phẩm (Tính Bằng USD)"),
                new DataColumn("Số lượng"),
                new DataColumn("Mô Tả")
            });
            foreach (var sanpham in list)
            { 
                string danhmuc = "";
                foreach (var spvdm in _spvdm.GetList(u => u.spid == sanpham.SanPhamId)) danhmuc = danhmuc + "/" + _danhmuc.Get(u => u.DanhMucId == spvdm.dmid).Name;
                danhmuc=danhmuc.TrimStart('/');
                dataTable.Rows.Add(sanpham.Name, sanpham.Author, danhmuc, sanpham.Price, sanpham.Number, sanpham.Description);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                }
            }
        }
        public FileResult Export(string SanPhamId)
        {
            var listid = SanPhamId.TrimEnd(',');
            var idList = listid.Split(',').Select(int.Parse).ToList();
            var list = new List<SanPham>();
            foreach (var id in idList)
            {
                list.Add(_sanpham.Get(u=>u.SanPhamId == id));
            }
            return ExportExcel("danhsachsanpham.xlsx", list);
        }
        public IActionResult ImportExcel()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ImportExcel(IFormFile? Inputfile)
        {
            if (_danhmuc.GetAll().Count()<=0)
            {
                TempData["error"] = "Chưa đủ dữ liệu để nhập file Excel!";
                return View();
            }
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            if (Inputfile != null)
            {
                var uploadfolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\ExcelUpload\\";
                if (!Directory.Exists(uploadfolder))
                {
                    Directory.CreateDirectory(uploadfolder);
                }
                var filepath = Path.Combine(uploadfolder, Inputfile.FileName);
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    await Inputfile.CopyToAsync(stream);
                }
                using (var stream = System.IO.File.Open(filepath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        do
                        {
                            bool isheaderskipped = false;
                            while (reader.Read())
                            {
                                if (!isheaderskipped)
                                {
                                    isheaderskipped = true;
                                    continue;
                                }
                                SanPham sanPham = new SanPham();
                                sanPham.Name = reader.GetValue(0).ToString();
                                sanPham.Author = reader.GetValue(1).ToString();
                                sanPham.Price = float.Parse(reader.GetValue(3).ToString());
                                sanPham.Number = int.Parse(reader.GetValue(4).ToString());
                                sanPham.Description = reader.GetValue(5).ToString();
                                string danhmucs=reader.GetValue(2).ToString();
                                var listdanhmuc = danhmucs.Split("/");
                                sanPham.DanhMucId = _danhmuc.Get(u => u.Name == listdanhmuc.LastOrDefault().ToString()).DanhMucId;
                                _sanpham.Add(sanPham);
                                _sanpham.Save();
                                foreach (var item in listdanhmuc)
                                {
                                    SanPhamVaDanhMuc entity = new SanPhamVaDanhMuc();
                                    entity.dmid = _danhmuc.Get(u => u.Name == item.ToString()).DanhMucId;
                                    entity.spid = sanPham.SanPhamId;
                                    _spvdm.Add(entity);
                                }
                                _spvdm.Save();
                            }
                        } while (reader.NextResult());
                    }
                }
                TempData["success"] = "File được transfer thành công qua cơ sở dữ liệu";
                if (System.IO.File.Exists(filepath)) System.IO.File.Delete(filepath);
            }
            return RedirectToAction("Index");
        }
    }
}


