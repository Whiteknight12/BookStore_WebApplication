// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using KeBanSach.Models.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KeBanSach.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private IWebHostEnvironment _webHostEnvironment;
        string wwwroot;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IWebHostEnvironment webHostEnvironment
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            [Display(Name ="Địa Chỉ")]
            public string Address { get; set; }
            [Display(Name ="Họ Và Tên")]
            public string Name { get; set; }
            public string? AvatarUrl { get; set; }
        }

        private async Task LoadAsync(CustomedUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var address = user.Address;
            var name=user.Name;
            var AvatarUrl = user.UserImgUrl;

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Address = address,
                Name=name,
                AvatarUrl = AvatarUrl
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var true_user=(CustomedUser)user;
            if (true_user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            await LoadAsync(true_user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile? UserAvatar)
        {
            var user = await _userManager.GetUserAsync(User);
            var true_user = (CustomedUser)user;
            if (true_user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(true_user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(true_user);
            var address=true_user.Address;
            var name=true_user.Name;

            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(true_user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            if (Input.Address != address)
            {
                true_user.Address = Input.Address;
                var updateResult = await _userManager.UpdateAsync(true_user);
                if (!updateResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set address.";
                    return RedirectToPage();
                }
            }
            if (Input.Name!=name)
            {
                true_user.Name = Input.Name;
                var updateResult=await _userManager.UpdateAsync(true_user);
                if (!updateResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set name.";
                    return RedirectToPage();
                }
            }
            if (UserAvatar != null)
            {
                wwwroot = _webHostEnvironment.WebRootPath;
                string filename = Path.GetFileNameWithoutExtension(UserAvatar.FileName) + Path.GetExtension(UserAvatar.FileName);
                string filepath = Path.Combine(wwwroot, @"images\");
                if (!string.IsNullOrEmpty(true_user.UserImgUrl))
                {
                    var oldimagepath = Path.Combine(wwwroot, true_user.UserImgUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldimagepath)) System.IO.File.Delete(oldimagepath);
                }
                using (var filestream = new FileStream(Path.Combine(filepath, filename), FileMode.Create))
                {
                    UserAvatar.CopyTo(filestream);
                }
                true_user.UserImgUrl = @"\images\" + filename;
                Input.AvatarUrl = true_user.UserImgUrl;
            }
            var Result = await _userManager.UpdateAsync(true_user);
            if (!Result.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to set name.";
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(true_user);
            StatusMessage = "Thông tin cập nhật thành công!";
            return RedirectToPage();
        }
    }
}
