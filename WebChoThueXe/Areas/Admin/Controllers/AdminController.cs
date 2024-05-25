using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebChoThueXe.Models;
using WebChoThueXe.Repository;

namespace WebChoThueXe.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly DataContext _context;
        public AdminController(UserManager<UserModel> userManager, DataContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            List<IdentityRole> roles;
            roles = await _context.Roles.ToListAsync();
            ViewBag.Role = new SelectList(roles, "Id", "Name");

            var query = from user in _context.Users
                        join userRoles in _context.UserRoles on user.Id equals userRoles.UserId
                        join role in _context.Roles on userRoles.RoleId equals role.Id
                        select new
                        {
                            UserId = user.Id,
                            user.UserName,
                            role.Name,
                        };
            var list = await query.ToListAsync();
            return View(list);
        }
    }
}
