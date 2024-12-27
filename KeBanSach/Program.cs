using KeBanSach.DataAccess.Data;
using KeBanSach.DataAccess.Data.Repository;
using KeBanSach.DataAccess.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using KeBanSach.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using KeBanSach.Models.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<ISanPham, SanPhamRepository>();
builder.Services.AddScoped<IDanhMuc, DanhMucRepository>();
builder.Services.AddScoped<ICart, CartRepository>();
builder.Services.AddScoped<IBoughtCart, BoughtCartRepository>();
builder.Services.AddScoped<IDaMua, DaMuaRepository>();
builder.Services.AddScoped<IDanhGia, DanhGiaRepository>();
builder.Services.AddScoped<ISellCanvas, SellCanvasRepository>();
builder.Services.AddScoped<ISanPhamVaDanhMuc, SanPhamVaDanhMucRepository>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddHttpClient<APIModel>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
