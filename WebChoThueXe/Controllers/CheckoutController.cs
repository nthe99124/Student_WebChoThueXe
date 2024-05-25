using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebChoThueXe.Models;
using WebChoThueXe.Models.ViewModels;
using WebChoThueXe.Repository;
using WebChoThueXe.Services;

namespace WebChoThueXe.Controllers
{
	[Authorize(Roles = "User")]
	public class CheckoutController : Controller
	{
		private readonly DataContext _dataContext;
		private readonly IVnPayService _vnPayservice;
		public CheckoutController(DataContext context, IVnPayService vnPayservice)
		{
			_dataContext = context;
			_vnPayservice = vnPayservice;
		}

		[HttpPost]
		public async Task<IActionResult> Checkout(OrderModel model, string payment = "COD")
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			if (payment == "Thanh toán VNPay")
			{
				var vnPayModel = new VnPaymentRequestModel
				{
					Amount = (double)cartItems.Sum(p => p.Total),
					CreatedDate = DateTime.Now,
					OrderId = new Random().Next(1000, 100000)
				};
				var ordercode = Guid.NewGuid().ToString();
				var orderItem = new OrderModel();

				orderItem.OrderCode = ordercode;
				orderItem.UserName = userEmail;
				orderItem.Status = 1;
				orderItem.CreatedDate = DateTime.Now;
				_dataContext.Add(orderItem);
				_dataContext.SaveChanges();

				foreach (var cart in cartItems)
				{
					var orderdetails = new OrderDetails();
					orderdetails.UserName = userEmail;
					orderdetails.OrderCode = ordercode;
					orderdetails.ProductId = (int)cart.ProductId;
					orderdetails.Price = cart.Price;
					orderdetails.Quantity = cart.Quantity;
					_dataContext.Add(orderdetails);
					_dataContext.SaveChanges();
				}
				return Redirect(_vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel));

			}


			if (userEmail == null)
			{
				return RedirectToAction("Login", "Account");
			}
			else
			{
				var ordercode = Guid.NewGuid().ToString();
				var orderItem = new OrderModel();

				orderItem.OrderCode = ordercode;
				orderItem.UserName = userEmail;
				orderItem.Status = 1;
				orderItem.CreatedDate = DateTime.Now;
				_dataContext.Add(orderItem);
				_dataContext.SaveChanges();

				foreach (var cart in cartItems)
				{
					var orderdetails = new OrderDetails();
					orderdetails.UserName = userEmail;
					orderdetails.OrderCode = ordercode;
					orderdetails.ProductId = (int)cart.ProductId;
					orderdetails.Price = cart.Price;
					orderdetails.Quantity = cart.Quantity;
					_dataContext.Add(orderdetails);
					_dataContext.SaveChanges();
				}
				TempData["success"] = "Đơn Hàng Đã Được Tạo,Vui Lòng Chờ Duyệt Đơn Hàng";
				return RedirectToAction("Index", "Cart");
			}

			return View();
		}
	}
}
