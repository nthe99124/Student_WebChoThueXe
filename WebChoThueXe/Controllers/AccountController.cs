using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebChoThueXe.Models;
using WebChoThueXe.Models.ViewModels;

namespace WebChoThueXe.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUserModel> _userManage;
		private SignInManager<AppUserModel> _signInManager;

        public AccountController(SignInManager<AppUserModel> signInManager, UserManager<AppUserModel> userManage) 
        {
			_signInManager = signInManager;
            _userManage = userManage;

		}
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl});
        }

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginVM)
		{
			if(ModelState.IsValid)
			{
				Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginVM.Username,loginVM.Password,false,false);

				if (result.Succeeded)
				{
					var user = await _userManage.FindByNameAsync(loginVM.Username);
					var role = await _userManage.GetRolesAsync(user);
					// kiểm tra role để điều hướng
					if (role != null && role.Contains("Admin"))
					{
						return Redirect(loginVM.ReturnUrl ?? "/admin/");
					}
					else
					{
						return Redirect(loginVM.ReturnUrl ?? "/");
					}	
				}
				ModelState.AddModelError("", " Username Hoặc Password Bị Sai");
			}
			return View(loginVM);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(UserModel user)
		{
			if (ModelState.IsValid)
			{
				AppUserModel newUser = new AppUserModel { UserName = user.Username, Email = user.Email};
				IdentityResult result = await _userManage.CreateAsync(newUser,user.Password);

				// xử lý role
				await _userManage.AddToRoleAsync(newUser, "User");
				if (result.Succeeded)
				{
					TempData["success"] = "Tạo User Thành Công.";
					return Redirect("/account/login");
				}
				foreach(IdentityError error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
			return View(user);
		}

		[Authorize(Roles = "User,Admin")]
		public async Task<IActionResult> Logout(string returnUrl = "/")
		{
			await _signInManager.SignOutAsync();
			return Redirect(returnUrl);
		}

	}
}
