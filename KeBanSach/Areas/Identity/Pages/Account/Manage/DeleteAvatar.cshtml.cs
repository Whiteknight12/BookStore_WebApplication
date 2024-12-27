using KeBanSach.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KeBanSach.Areas.Identity.Pages.Account.Manage
{
    public class DeleteAvatarModel : PageModel
    {
        private UserManager<IdentityUser> _usermanager;
        string wwwroot;
        private IWebHostEnvironment _webHostEnvironment;
        public DeleteAvatarModel(UserManager<IdentityUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _usermanager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPost(string UserId)
        {
            var user=await _usermanager.FindByIdAsync(UserId);
            var true_user = (CustomedUser)user;
            if (!string.IsNullOrEmpty(true_user.UserImgUrl))
            {
                wwwroot = _webHostEnvironment.WebRootPath;
                var oldimagepath = Path.Combine(wwwroot, true_user.UserImgUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldimagepath)) System.IO.File.Delete(oldimagepath);
                true_user.UserImgUrl = "";
                await _usermanager.UpdateAsync(true_user);
            }
            return RedirectToPage("/Account/Manage/Index");
        }
    }
}
