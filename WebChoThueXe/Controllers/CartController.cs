using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebChoThueXe.Models;
using WebChoThueXe.Models.ViewModels;
using WebChoThueXe.Repository;
using WebChoThueXe.Services;

namespace WebChoThueXe.Controllers
{
	public class CartController : Controller
    {
        private readonly DataContext _dataContext;
		private readonly IVnPayService _vnPayservice;

		public CartController(DataContext _context, IVnPayService vnPayservice)
        {
            _dataContext = _context;
			_vnPayservice = vnPayservice;
		}
		
        [Authorize(Roles = "User")]
		public IActionResult Index()
        {
            List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemViewModels cartVM = new()
            {
                CartItems = cartItems,
                GrandTotal = cartItems.Sum(x => x.Quantity * x.Price),
            };

            return View(cartVM);
        }

        [Authorize(Roles = "User")]
        public ActionResult Checkout()
        {
            return View("~/Views/Checkout/Index.cshtml");
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Add(int Id)
        {
            await AddToCard(Id);

            TempData["success"] = "Thêm Sản Phẩm Thành Công";

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> BuyNow(int Id)
        {
            await AddToCard(Id);

            return RedirectToAction("Index");
        }

		[Authorize(Roles = "User")]
		public async Task<JsonResult> AddToCardJson(int Id)
		{
			await AddToCard(Id);
			TempData["success"] = "Thêm Sản Phẩm Thành Công";
			return Json(new {success = true});
		}

		[Authorize(Roles = "User")]
        public async Task<IActionResult> Decrease(int Id)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");

            CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();

            if (cartItem.Quantity > 1)
            {
                --cartItem.Quantity;
            }
            else
            {
                cart.RemoveAll(p => p.ProductId == Id);
            }
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
			TempData["success"] = "Xoá 1 Sản Phẩm Thành Công";

			return RedirectToAction("Index");
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Increase(int Id)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");

            CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();

            if (cartItem.Quantity >= 1)
            {
                ++cartItem.Quantity;
            }
            else
            {
                cart.RemoveAll(p => p.ProductId == Id);
            }
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
			TempData["success"] = "Thêm 1 Sản Phẩm Thành Công";
			return RedirectToAction("Index");
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Remove(int Id)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");

            cart.RemoveAll(p => p.ProductId == Id);
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
			TempData["success"] = "Xoá Sản Phẩm Thành Công";
			return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
		public async Task<IActionResult> Clear()
		{
			HttpContext.Session.Remove("Cart");
			TempData["success"] = "Xoá Toàn Bộ Sản Phẩm Thành Công";
			return RedirectToAction("Index");
		}
        
        [Authorize(Roles = "User")]
		[HttpPost]
		public IActionResult PaymentSuccess()
		{
			return View("Success");
		}

        [Authorize(Roles = "User")]
		public IActionResult PaymentFail()
		{
			return View();
		}

		public IActionResult PaymentCallBack()
		{
            var response = _vnPayservice.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["error"] = $"Lỗi thanh toán VN Pay: {response.VnPayResponseCode}";
                return RedirectToAction("Index");
            }

            TempData["success"] = $"Thanh toán VNPay thành công";
			return RedirectToAction("Index");
		}

        private async Task AddToCard(int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemModel cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();

            if (cartItems == null)
            {
                cart.Add(new CartItemModel(product));
            }
            else
            {
                cartItems.Quantity += 1;
            }

            HttpContext.Session.SetJson("Cart", cart);
        }
	}
}
