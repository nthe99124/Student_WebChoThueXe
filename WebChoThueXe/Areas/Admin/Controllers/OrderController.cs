﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebChoThueXe.Models;
using WebChoThueXe.Repository;

namespace WebChoThueXe.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class OrderController : Controller
	{
        private readonly DataContext _dataContext;
        public OrderController(DataContext context)
        {
            _dataContext = context;

        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Orders.OrderByDescending(p => p.Id).ToListAsync());
        }
        public async Task<IActionResult> ViewOrder(string ordercode)
        {
            var DetailsOrder  = await _dataContext.OrderDetails.Include(od=>od.Product).Where(od=>od.OrderCode==ordercode).ToListAsync();
            return View(DetailsOrder);
        }
    }
}
