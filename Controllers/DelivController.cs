using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaqrResturant.Data;
using SaqrResturant.Models;

namespace SaqrResturant.Controllers
{
    public class DelivController : Controller
    {
        
            //context querier
            private readonly ResturantContext _context;
            //context querier creation
            public DelivController(ResturantContext context)
            {
                _context = context;
            }

            [HttpGet]
        public IActionResult DeliveryViewOrders()
        {
            var deliveries = _context.Deliveries
                .Include(d => d.Order)
                    .ThenInclude(o => o.User)
                .Include(d => d.Order)
                    .ThenInclude(o => o.OrderDetails)
                        .ThenInclude(od => od.Menu)
                .ToList();
            // passing this will not work, i dont know why

            // so we create an intermediary model
            var model = deliveries.Select(d => new DeliveryOrderViewModel
            {
                DeliveryId = d.Id ?? 0,
                OrderId = d.OrderId ?? 0,

                CustomerName = d.Order?.User?.userName ?? "Unknown",

                Address = d.Order?.location ?? "",

                Items = string.Join(" , ",
                    d.Order?.OrderDetails?
                        .Where(od => od.Menu != null)
                        .Select(od => od.quantity + "x " + od.Menu.name)
                    ?? Enumerable.Empty<string>()
                ),

                TotalPrice = d.Order?.totalPrice + 3 ?? 0,

                Status = d.status ?? Status.Pending
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult ChangeStatus(Status deliverystatus,int id)
        {
            var driverstring = HttpContext.Session.GetString("Username");
            if (driverstring == null) return RedirectToAction("Login", "Auth");
            var driver = _context.Users.FirstOrDefault(d => d.userName == driverstring);
            if (driver == null) return RedirectToAction("Login", "Auth");

            if (driver.role == Models.Role.User) return RedirectToAction("Index", "Home");

            var delivery = _context.Deliveries.FirstOrDefault(d=>d.Id == id);
            if (delivery == null) return RedirectToAction("DeliveryViewOrders", "Deliv");
            delivery.status = deliverystatus;
            delivery.DeliveryUserId = driver.Id;
            _context.SaveChanges();



            return RedirectToAction("DeliveryViewOrders","Deliv");

        }

    }
}
