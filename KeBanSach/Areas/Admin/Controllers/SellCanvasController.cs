using KeBanSach.DataAccess.Data.Repository.IRepository;
using KeBanSach.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using System.Runtime.CompilerServices;

namespace KeBanSach.Areas.Admin.Controllers
{
    public class SellCanvasController : Controller
    {
        private ISellCanvas _sellcanvas;
        private ISanPham _sanpham;
        private IDanhMuc _danhmuc;
        public SellCanvasController(ISellCanvas sellCanvas, ISanPham sanPham, IDanhMuc danhMuc)
        {
            _sellcanvas = sellCanvas;
            _sanpham = sanPham;
            _danhmuc = danhMuc;
        }
        [Route("SellCanvas/SalesData")]
        public IActionResult SalesData(DateTime? startDate, DateTime? endDate)
        {
            var listsellcanvas = _sellcanvas.GetAll();

            if (!startDate.HasValue || !endDate.HasValue)
            {
                DateTime currentDate = DateTime.Now;
                startDate = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(-4);
                endDate = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(1).AddDays(-1);
            }
            else
            {
                startDate = new DateTime(startDate.Value.Year, startDate.Value.Month, 1);
                endDate = new DateTime(endDate.Value.Year, endDate.Value.Month, 1).AddMonths(1).AddDays(-1);
            }
            listsellcanvas = listsellcanvas.Where(x => x.Time >= startDate.Value && x.Time <= endDate.Value).ToList();

            foreach (var item in listsellcanvas)
            {
                item.DanhMuc = _danhmuc.Get(u => u.DanhMucId == item.dmid);
                item.SanPham = _sanpham.Get(u => u.SanPhamId == item.spid);
            }

            var salesByMonth = listsellcanvas
                .GroupBy(s => new { Month = s.Time.Month, Year = s.Time.Year })
                .Select(g => new
                {
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    TotalSales = g.Sum(x => x.Number)
                })
                .OrderBy(u => u.Month)
                .ToList();

            var topCategories = listsellcanvas
                .GroupBy(s => s.DanhMuc.Name)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalSales = g.Sum(x => x.Number)
                })
                .OrderByDescending(x => x.TotalSales)
                .Take(1)
                .ToList();

            return Json(new { SalesByMonth = salesByMonth, TopCategories = topCategories });
        }
    }
}
