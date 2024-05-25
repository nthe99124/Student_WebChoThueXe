using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Drawing2D;
using WebChoThueXe.Models;
using WebChoThueXe.Repository;

namespace WebChoThueXe.Controllers
{
    public class CategoryController : Controller
    {
		private UserManager<AppUserModel> _userManage;
		private readonly DataContext _dataContext;
        public CategoryController(DataContext context, UserManager<AppUserModel> userManage)
		{
			_dataContext = context;
			_userManage = userManage;
		}

		public async Task<IActionResult> Index(string Slug = "")
        {
            CategoryModel category = _dataContext.Categories.Where(c => c.Slug == Slug).FirstOrDefault();

            if (category == null) return RedirectToAction("Index");

			var userId = _userManage.GetUserId(User);

			var productByBrand = from p in _dataContext.Products
								 join b in _dataContext.Brands on p.BrandId equals b.Id
								 join c in _dataContext.Categories on p.CategoryId equals c.Id
								 where p.CategoryId == category.Id
								 orderby p.Id descending
								 select new ProductViewModel
								 {
									 Id = p.Id,
									 Name = p.Name,
									 Slug = p.Slug,
									 Description = p.Description,
									 Price = p.Price,
									 BrandId = b.Id,
									 CategoryId = p.CategoryId,
									 BrandName = b.Name,
									 CategoryName = c.Name,
									 Image = p.Image,
									 IsFavorite = _dataContext.ProductFavorite.Any(pf => pf.ProductId == p.Id && pf.AccountId == userId),
									 Star = _dataContext.Ratings.Where(r => r.ProductId == p.Id).Average(r => (double?)r.Star) ?? 0
								 };

			return View(productByBrand);
        }
    }
}
