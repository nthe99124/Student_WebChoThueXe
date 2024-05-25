using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using WebChoThueXe.Models;
using WebChoThueXe.Models.ViewModels;
using WebChoThueXe.Repository;

namespace WebChoThueXe.Controllers
{
	public class ProductController : Controller
	{
		private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private UserManager<AppUserModel> _userManage;
        public ProductController(DataContext context, IWebHostEnvironment webHostEnvironment, UserManager<AppUserModel> userManage)
		{
			_dataContext = context;
            _webHostEnvironment = webHostEnvironment;
            _userManage = userManage;
        }
		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Details(int Id)
		{
			var productById = from p in _dataContext.Products
							  join b in _dataContext.Brands on p.BrandId equals b.Id
							  join c in _dataContext.Categories on p.CategoryId equals c.Id
							  where p.Id == Id
							  select new ProductDetailVM
							  {
                                  Id = p.Id,
								  Name = p.Name,
                                  Description = p.Description,
								  Price = p.Price,
								  BrandName = b.Name,
								  CategoryName = c.Name,
								  Image = p.Image,
							  };

			// lấy thông tin đánh giá
			var ratings = _dataContext.Ratings.Where(item => item.ProductId == Id).OrderByDescending(item => item.CreatedDate).ToList();

			var totalStar = 0;
			var totalStar1 = 0;
			var totalStar2 = 0;
			var totalStar3 = 0;
			var totalStar4 = 0;
			var totalStar5 = 0;
			var listImg = new List<string>();
			if (ratings.Count > 0)
			{
				foreach (var item in ratings)
				{
					switch (item.Star)
					{
						case 1:
							totalStar1++;
							break;
						case 2:
							totalStar2++;
							break;
						case 3:
							totalStar3++;
							break;
						case 4:
							totalStar4++;
							break;
						case 5:
							totalStar5++;
							break;
					}
					totalStar += item.Star;
					if (!string.IsNullOrEmpty(item.LinkImage1))
					{
						listImg.Add(item.LinkImage1);
					}
					if (!string.IsNullOrEmpty(item.LinkImage2))
					{
						listImg.Add(item.LinkImage2);
					}
					if (!string.IsNullOrEmpty(item.LinkImage3))
					{
						listImg.Add(item.LinkImage3);
					}
				}
			}

			var result = new ProductDetailViewModel()
			{
				ProductDetail = productById.FirstOrDefault(),
				RatingInfor = new RatingVM
				{
					ListImg = listImg.Take(5).ToList(),
					TotalRate = ratings.Count(),
					Star = ratings.Count() == 0 ? 0 : (float)Math.Round(totalStar / (float)ratings.Count(), 2),
					PercentStar1 = ratings.Count() == 0 ? 0 : (float)Math.Round(totalStar1 / (float)ratings.Count() * 100, 2),
					PercentStar2 = ratings.Count() == 0 ? 0 : (float)Math.Round(totalStar2 / (float)ratings.Count() * 100, 2),
					PercentStar3 = ratings.Count() == 0 ? 0 : (float)Math.Round(totalStar3 / (float)ratings.Count() * 100, 2),
					PercentStar4 = ratings.Count() == 0 ? 0 : (float)Math.Round(totalStar4 / (float)ratings.Count() * 100, 2),
					PercentStar5 = ratings.Count() == 0 ? 0 : (float)Math.Round(totalStar5 / (float)ratings.Count() * 100, 2),
                    HasEnableRate = !ratings.Any(item => item.CreatedById == _userManage.GetUserId(User))
                }
			};

			return View(result);
		}

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<JsonResult> AddOrRemoveFavourite([FromForm]int productId)
        {
            var favoriteAdd = new ProductFavoriteModel()
            {
                ProductId = productId,
                AccountId = _userManage.GetUserId(User),
            };

			var favorite = _dataContext.ProductFavorite.FirstOrDefault(item => item.ProductId == favoriteAdd.ProductId && item.AccountId == favoriteAdd.AccountId);
			var isCheckFavorite = true;

            if (favorite != null)
			{
				isCheckFavorite = false;

				_dataContext.Remove(favorite);
            }
			else
			{
                _dataContext.Add(favoriteAdd);
            }
            
            await _dataContext.SaveChangesAsync();
            return Json(new { success = true, isCheckFavorite = isCheckFavorite });
        }

        private async Task<string> UploadFile(IFormFile file)
		{
            string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
            string imageName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadDir, imageName);

            FileStream fs = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fs);
            fs.Close();
			return imageName;
        }
    }
}
