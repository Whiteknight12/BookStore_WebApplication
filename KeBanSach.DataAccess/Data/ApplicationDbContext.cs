using KeBanSach.Models;
using KeBanSach.Models.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KeBanSach.DataAccess.Data
{
    public class ApplicationDbContext: IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) 
        {
            
        }
        public DbSet<DanhMuc> BangDanhMuc { get; set; }
        public DbSet<SanPham> BangSanPham { get; set; }  
        public DbSet<CustomedUser> BangUser {  get; set; }
        public DbSet<Cart> BangCart { get; set; }
        public DbSet<BoughtCart> BangBoughtCart { get; set; }
        public DbSet<DanhGia> BangDanhGia { get; set; }
        public DbSet<DaMua> BangDaMua { get; set; }
        public DbSet<SellCanvas> BangSellCanvas { get; set; }
        public DbSet<DanhMucCopy> BangDanhMucCopy { get; set; }
        public DbSet<SanPhamVaDanhMuc> BangSanPhamVaDanhMuc { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
