using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using WebChoThueXe.Models;
using WebChoThueXe.Models.ViewModels;
using WebChoThueXe.Models.ViewModels.StoriesProject.Model.ViewModel;
using WebChoThueXe.Repository;

namespace WebChoThueXe.Controllers
{
    public class RatingController : Controller
	{
		private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private UserManager<AppUserModel> _userManage;
        public RatingController(DataContext context, IWebHostEnvironment webHostEnvironment, UserManager<AppUserModel> userManage)
		{
			_dataContext = context;
            _webHostEnvironment = webHostEnvironment;
            _userManage = userManage;
        }
		public IActionResult Index()
		{
			return View();
		}

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<JsonResult> Rating([FromForm] RatingViewModel ratingViewModel, IFormFile file1, IFormFile file2, IFormFile file3)
        {
            var ratingAdd = new RatingModel()
            {
                Content = ratingViewModel.Content,
                Star = ratingViewModel.Star,
                CreatedByName = _userManage.GetUserName(User),
                CreatedById = _userManage.GetUserId(User),
                ProductId = ratingViewModel.ProductId,
            };

            if (file1 != null)
            {
                ratingAdd.LinkImage1 = await UploadFile(file1);
            }

            if (file2 != null)
            {
                ratingAdd.LinkImage2 = await UploadFile(file2);
            }

            if (file3 != null)
            {
                ratingAdd.LinkImage3 = await UploadFile(file3);
            }

            _dataContext.Add(ratingAdd);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Đánh giá Thành Công";
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<JsonResult> GetPagingRating(int pageIndex, int productId, int? star)
        {
            // mặc định 5 bản ghi trên trang
            var ratingListCondition = _dataContext.Ratings.Where(item => item.ProductId == productId && ((star != null && item.Star == star) || star == null));
            var pagingRating = ratingListCondition.Skip((pageIndex - 1) * 5).Take(5).ToList();
            var totalCount = ratingListCondition.Count();
            var res = new RestPagingOutput<RatingModel>();
            res.SuccessEventHandler(pagingRating, totalCount, pageIndex);
            return Json(res);
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
