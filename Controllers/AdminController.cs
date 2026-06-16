using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaqrResturant.Data;
using SaqrResturant.Models;

namespace SaqrResturant.Controllers
{
    public class AdminController : Controller
    {
        //context querier
        private readonly ResturantContext _context;
        //context querier creation
        public AdminController(ResturantContext context)
        {
            _context = context;
        }


        public IActionResult ManageMenu()
        {
            var userstring = HttpContext.Session.GetString("Username");
            if(userstring==null) return RedirectToAction("Login", "Auth");
            var user = _context.Users.FirstOrDefault(u=>u.userName==userstring);
            if(user==null || user.role!= Models.Role.Admin) return RedirectToAction("Login", "Auth");

            var menu = _context.Menu.ToList();
            if (menu == null) return RedirectToAction("Index", "Home");




            return View(menu);
        }
        [HttpGet]
        public IActionResult EditItem(int id)
        {
            var userstring = HttpContext.Session.GetString("Username");
            if (userstring == null) return RedirectToAction("Login", "Auth");
            var user = _context.Users.FirstOrDefault(u => u.userName == userstring);
            if (user == null || user.role != Models.Role.Admin) return RedirectToAction("Login", "Auth");


            var item = _context.Menu.FirstOrDefault(u=>u.Id==id);
            if (item == null) return RedirectToAction("Index", "Home");

            

            
            
            return View(item);
        }
        [HttpPost]
        public IActionResult EditItem(menuModel newmodel)
        {
            var userstring = HttpContext.Session.GetString("Username");
            if (userstring == null) return RedirectToAction("Login", "Auth");
            var user = _context.Users.FirstOrDefault(u => u.userName == userstring);
            if (user == null || user.role != Models.Role.Admin) return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid) return View(newmodel);

            var olditem = _context.Menu.FirstOrDefault(u=>u.Id==newmodel.Id);
            if(olditem==null) return RedirectToAction("Index", "Home");

            olditem.name = newmodel.name;
            olditem.price = newmodel.price;
            olditem.description = newmodel.description;
            olditem.Imgpath = newmodel.Imgpath;
            olditem.category = newmodel.category;




            _context.SaveChanges();
            TempData["message"] = "Item edited successfully";
            TempData["color"] = "green";
            return RedirectToAction("ManageMenu","Admin");
        }


        [HttpPost]
        public IActionResult DeleteItem(int id)
        {
            var userstring = HttpContext.Session.GetString("Username");
            if (userstring == null) return RedirectToAction("Login", "Auth");
            var user = _context.Users.FirstOrDefault(u => u.userName == userstring);
            if (user == null || user.role != Models.Role.Admin) return RedirectToAction("Login", "Auth");

            var item = _context.Menu.FirstOrDefault(u => u.Id == id);
            if(item==null) return RedirectToAction("ManageMenu", "Admin");

            _context.Menu.Remove(item);
            _context.SaveChanges();
            TempData["message"] = "Item deleted successfully";
            TempData["color"] = "red";
            return RedirectToAction("ManageMenu", "Admin");

        }

        [HttpGet]
        public IActionResult AddItem()
        {
            var userstring = HttpContext.Session.GetString("Username");
            if (userstring == null) return RedirectToAction("Login", "Auth");
            var user = _context.Users.FirstOrDefault(u => u.userName == userstring);
            if (user == null || user.role != Models.Role.Admin) return RedirectToAction("Login", "Auth");


            return View(new menuModel());
        }
        [HttpPost]
        public IActionResult AddItem(menuModel model)
        {
            var userstring = HttpContext.Session.GetString("Username");
            if (userstring == null) return RedirectToAction("Login", "Auth");
            var user = _context.Users.FirstOrDefault(u => u.userName == userstring);
            if (user == null || user.role != Models.Role.Admin) return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid) return View(model);

            var item = new menuModel
            {
                
                category = model.category,
                description = model.description,
                Imgpath = model.Imgpath,
                name = model.name,
                price = model.price
                 

            };
            _context.Menu.Add(item);
            _context.SaveChanges();


            return RedirectToAction("ManageMenu","Admin");
        }



        [HttpGet]
        public IActionResult ManageOrders()
        {
            var userstring = HttpContext.Session.GetString("Username");
            if (userstring == null) return RedirectToAction("Login", "Auth");
            var user = _context.Users.FirstOrDefault(u => u.userName == userstring);
            if (user == null || user.role != Models.Role.Admin) return RedirectToAction("Login", "Auth");

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

        public IActionResult Editstatus(int id,Status orderstatus)
        {
            var userstring = HttpContext.Session.GetString("Username");
            if (userstring == null) return RedirectToAction("Login", "Auth");
            var user = _context.Users.FirstOrDefault(u => u.userName == userstring);
            if (user == null || user.role != Models.Role.Admin) return RedirectToAction("Login", "Auth");

            var delivery = _context.Deliveries.FirstOrDefault(u=>u.Id==id);
            if (delivery == null) return RedirectToAction("ManageOrders","Admin");
            delivery.status = orderstatus;
            _context.SaveChanges();



            return RedirectToAction("ManageOrders","Admin");

        }


        [HttpGet]
        public IActionResult ManageUsers()
        {
            var userstring = HttpContext.Session.GetString("Username");
            if (userstring == null) return RedirectToAction("Login", "Auth");
            var user = _context.Users.FirstOrDefault(u => u.userName == userstring);
            if (user == null || user.role != Models.Role.Admin) return RedirectToAction("Login", "Auth");

            var users = _context.Users.Where(u=>u.deletedOn ==null && u.Id != user.Id).ToList();
            if (users == null) return RedirectToAction("Index", "Home");


            return View(users);
        }
        [HttpPost]
        public IActionResult Changeroles(int id,Role role)
        {
            var userstring = HttpContext.Session.GetString("Username");
            if (userstring == null) return RedirectToAction("Login", "Auth");
            var user = _context.Users.FirstOrDefault(u => u.userName == userstring);
            if (user == null || user.role != Models.Role.Admin) return RedirectToAction("Login", "Auth");

            var editeduser = _context.Users.FirstOrDefault(u=>u.Id == id);
            if (editeduser == null) return RedirectToAction("ManageUsers","Admin");
            editeduser.role = role;
            _context.SaveChanges();

            return RedirectToAction("ManageUsers","Admin");

        }

        [HttpGet]
        public IActionResult ChangeUserPassword(int id)
        {
            var userstring = HttpContext.Session.GetString("Username");
            if (userstring == null) return RedirectToAction("Login", "Auth");
            var user = _context.Users.FirstOrDefault(u => u.userName == userstring);
            if (user == null || user.role != Models.Role.Admin) return RedirectToAction("Login", "Auth");

            var userchange = _context.Users.FirstOrDefault(u=>u.Id==id);
            if (userchange == null) return RedirectToAction("ManageUsers","Admin");

            var password = new ChangepasswordModel
            {
                currentpassword = userchange.password,
                  id=id


            };

            return View(password);

        }
        [HttpPost]
        public IActionResult ChangeUserPassword(ChangepasswordModel model)
        {
            var userstring = HttpContext.Session.GetString("Username");
            if (userstring == null) return RedirectToAction("Login", "Auth");
            var user = _context.Users.FirstOrDefault(u => u.userName == userstring);
            if (user == null || user.role != Models.Role.Admin) return RedirectToAction("Login", "Auth");
            
            if(!ModelState.IsValid) return View(model);

            var userchange = _context.Users.FirstOrDefault(u => u.Id == model.id);
            if (userchange == null) return RedirectToAction("ManageUsers", "Admin");

            if(model.currentpassword != userchange.password)
            {
                TempData["message"] = "Current Password is wrong";
                return RedirectToAction("ChangeUserPassword","Admin");
            }
            userchange.password = model.newpassword;
            _context.SaveChanges();

            TempData["message"] = "Changed user password successfully";
            return RedirectToAction("ManageUsers","Admin");

        }
        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            var userstring = HttpContext.Session.GetString("Username");
            if (userstring == null) return RedirectToAction("Login", "Auth");
            var user = _context.Users.FirstOrDefault(u => u.userName == userstring);
            if (user == null || user.role != Models.Role.Admin) return RedirectToAction("Login", "Auth");

            var deleteduser = _context.Users.FirstOrDefault(u => u.Id == id);
            if (deleteduser == null) return RedirectToAction("ManageUers","Admin");

            deleteduser.deletedOn= DateTime.Now;
            deleteduser.password=Guid.NewGuid().ToString();

            _context.SaveChanges();
            TempData["message"] = "User deleted successfully";
            return RedirectToAction("ManageUsers","Admin");
        }


    }
}
