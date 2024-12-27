using KeBanSach.DataAccess.Data.Repository.IRepository;
using KeBanSach.Models;
using KeBanSach.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace KeBanSach.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    [Area("Admin")]
    public class DanhMucController : Controller
    {
        private IDanhMuc _danhmuc;
        private ISanPham _sanpham;
        private ISanPhamVaDanhMuc _spvdm;
        public DanhMucController(IDanhMuc danhMuc, ISanPham sanpham, ISanPhamVaDanhMuc spvdm)
        {
            _danhmuc = danhMuc;
            _sanpham = sanpham;
            _spvdm = spvdm;
        }
        public IActionResult Index(List<int> DeletedDanhMucId)
        {
            if (DeletedDanhMucId.Count>0)
            {
                foreach (var id in DeletedDanhMucId)
                {
                    var deletedanhmuc = _danhmuc.Get(u => u.DanhMucId == id);
                    var sanpham = _sanpham.Get(u => u.DanhMucId == deletedanhmuc.DanhMucId);
                    if (sanpham != null)
                    {
                        TempData["error"] = "Không Thể Xóa, Danh Mục Đang Được Sử Dụng";
                        return RedirectToAction("Index");
                    }
                }
                foreach (var id in DeletedDanhMucId)
                {
                    var deletedanhmuc = _danhmuc.Get(u => u.DanhMucId == id);
                    _danhmuc.Delete(deletedanhmuc);
                }
                TempData["success"] = "Xóa Danh Mục Thành Công!";
                _danhmuc.Save();
            }
            return View(_danhmuc.GetAll().ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(DanhMuc obj)
        {
            if (obj.Name == obj.DanhMucId.ToString()) ModelState.AddModelError("Name", "Ten cua danh muc khong duoc trung voi Id cua danh muc");
            if (ModelState.IsValid)
            {
                _danhmuc.Add(obj);
                _danhmuc.Save();
                TempData["success"] = "Danh Mục Mới Đã Được Tạo";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            DanhMuc obj = _danhmuc.Get(u => u.DanhMucId == id);
            return View(obj);
        }
        [HttpPost]
        public IActionResult Edit(DanhMuc obj)
        {
            if (obj.Name == obj.DanhMucId.ToString()) ModelState.AddModelError("Name", "Ten cua danh muc khong duoc trung voi Id cua danh muc");
            if (ModelState.IsValid)
            {
                _danhmuc.Update(obj);
                _danhmuc.Save();
                TempData["success"] = "Danh Mục Đã Được Sửa";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            return View(_danhmuc.Get(u => u.DanhMucId == id));
        }
        [HttpPost]
        public IActionResult Delete(DanhMuc obj)
        {
            var listsanpham = _spvdm.GetList(u => u.dmid == obj.DanhMucId);
            if (listsanpham.Count()>0)
            {
                TempData["error"] = "Không Thể Xóa, Danh Mục Đang Được Sử Dụng";
                return RedirectToAction("Index");
            }
            _danhmuc.Delete(obj);
            _danhmuc.Save();
            TempData["success"] = "Danh Mục Đã Được Xóa";
            return RedirectToAction("Index");
        }
    }
}
