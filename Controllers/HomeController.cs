using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaqrResturant.Data;
using SaqrResturant.Models;
using System.Diagnostics;
using System.Text.Json;


// TODO: 1.Add Item Editing
namespace SaqrResturant.Controllers
{
    public class HomeController : Controller
    {
        //context querier
        private readonly ResturantContext _context;
        //context querier creation
        public HomeController(ResturantContext context)
        {
            _context = context;
        }



        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Cart()
        {
            if (HttpContext.Session.GetString("Username") == null) return RedirectToAction("Login","Auth");

            var cartjson = HttpContext.Session.GetString("Cart");
            List<CartItemModel> cart;

            if (string.IsNullOrEmpty(cartjson)) {

               cart= new List<CartItemModel>();
            
            
            }
            else
            {
                cart = JsonSerializer.Deserialize<List<CartItemModel>>(cartjson) ?? new List<CartItemModel>();

            }




            // all theses comments are made by 202310352
            // gets all the item ids in the cart and puts them in a list
            var ids = cart.Select(x=>x.MenuID).ToList();

            // get the necessary menu items in a list
            var menuitems=_context.Menu.Where(u=>ids.Contains(u.Id)).ToList();

            foreach(var item in cart) {

                var temp = menuitems.FirstOrDefault(u=> u.Id== item.MenuID);
            

                if(temp != null)
                {
                    item.Price = temp.price;
                    item.name= temp.name;
                    item.Imgpath= temp.Imgpath;



                }
            
            
            }
            // now we have all the items we need to display in cart

            HttpContext.Session.SetString("Cart",JsonSerializer.Serialize(cart));

            return View(cart);
        }
        [HttpPost]
        public IActionResult Removeitem(int id) {

            var cartjson = HttpContext.Session.GetString("Cart");
            List<CartItemModel> cart;

            if (string.IsNullOrEmpty(cartjson))
            {

                cart = new List<CartItemModel>();


            }
            else
            {
                cart = JsonSerializer.Deserialize<List<CartItemModel>>(cartjson) ?? new List<CartItemModel>();

            }

            var item = cart.FirstOrDefault(u=> u.MenuID == id);
            if (item != null) { 
                cart.Remove(item);
                HttpContext.Session.SetString("Cart",JsonSerializer.Serialize(cart));
            
            }

            return RedirectToAction("Cart","Home");
        }

        [HttpGet]
        public IActionResult Contact()
        {
            var username = HttpContext.Session.GetString("Username");
            if (username != null)
            {
                var user = _context.Users.FirstOrDefault(u=>u.userName==username);
                if(user !=null)return View(new ContactUsModel{fullname = user.fullName ?? "",UserId= user.Id});
            }

            return View();
        }
        [HttpPost]
        public IActionResult Contact(ContactUsModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var username = HttpContext.Session.GetString("Username");
            if (username != null)
            {
                var user = _context.Users.FirstOrDefault(u => u.userName == username);
                if (user != null)
                {
                    model.UserId = user.Id;
                }
            }
            _context.Inbox.Add(model);
            _context.SaveChanges();

            TempData["message"] = "Thank you for your Inquiry";
                return RedirectToAction("Contact","Home");
        }
        public IActionResult ItemDetails(int id)
        {
            var item= _context.Menu.FirstOrDefault(x => x.Id == id);

            if (item == null) { return RedirectToAction("Menu","Home"); }

            return View(item);
        }
        [HttpGet]
        public IActionResult Menu()
        {
            var menuitems = _context.Menu.ToList();
            ViewBag.category = "All";


            return View(menuitems);
        }
        [HttpPost]
        public IActionResult Menu(string? category)
        {
            var item = _context.Menu.AsQueryable();

            // attempting to convert category of item to string crashes the server, instead we parse the input category to an enum,
            // this function takes categories type as input and outputs the corrosponding category, if none equal, outputs "false"
            Enum.TryParse<Categories>(category, out var parsedcategory);


            if (!string.IsNullOrEmpty(category)) {
                item = item.Where(u=> u.category == parsedcategory);
                ViewBag.category = category;
            
            }
            else { ViewBag.category = "All"; }


            return View(item.ToList());
        }

        [HttpPost]
        public IActionResult Search(string? query)
        {
            if (string.IsNullOrEmpty(query)) {  return RedirectToAction("Menu","Home"); }
            var items = _context.Menu.Where(u => u.name !=null && u.name.ToLower().Contains(query.ToLower())).ToList();

            if(items == null) return RedirectToAction("Menu", "Home");

            return View("Menu",items);
        }


        [HttpPost]  
        public IActionResult Additemtocart(int id, int quantity)
        {
            if (HttpContext.Session.GetString("Username") == null) return RedirectToAction("Login","Auth");
            var item = _context.Menu.FirstOrDefault(u=> u.Id == id);
            if(item == null) return RedirectToAction("Menu", "Home");
            var user= _context.Users.FirstOrDefault(u=> u.userName == HttpContext.Session.GetString("Username"));

            // item exists and user exists, create session cart order

            var cartJson = HttpContext.Session.GetString("Cart"); // checking for cart

            List<CartItemModel> cart;

            if (string.IsNullOrEmpty(cartJson))
            {
                 cart= new List<CartItemModel>();


            }
            else
            {
                // ?? operator returns the left side if not null, right side if null, added because the compiler is paranoid

                cart = JsonSerializer.Deserialize<List<CartItemModel>>(cartJson) ?? new List<CartItemModel>();
            }
            var itemexists = cart.FirstOrDefault(u=>u.MenuID == id);


            if (itemexists != null)
            {
                itemexists.Quantity += quantity;


            }
            else
            {
                cart.Add(new CartItemModel { MenuID=id, Quantity=quantity});


            }




            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
            TempData["message"] = "Item added successfully, Please check your Cart";
            return RedirectToAction("Menu","Home");
        }
        [HttpGet]
        public IActionResult Edititemquantity(int id)
        {
            var item = _context.Menu.FirstOrDefault(u=> u.Id == id);
            if (item == null) return RedirectToAction("Cart","Home");



            return View(item);
        }
        [HttpPost]
        public IActionResult Edititemquantity(int id, int quantity)
        {
            var cartjson = HttpContext.Session.GetString("Cart");
            List<CartItemModel> cart;

            if (string.IsNullOrEmpty(cartjson))
            {

                cart = new List<CartItemModel>();


            }
            else
            {
                cart = JsonSerializer.Deserialize<List<CartItemModel>>(cartjson) ?? new List<CartItemModel>();

            }

            var item = cart.FirstOrDefault(u=>u.MenuID==id);

            // extra check
            if (item == null) return RedirectToAction("Cart","Home");
            item.Quantity = quantity;


            HttpContext.Session.SetString("Cart",JsonSerializer.Serialize(cart));
            return RedirectToAction("Cart","Home");
        }
        [HttpGet]
        public IActionResult Payment()
        {
            var username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login","Auth");
            double? total = 0;

            var cartjson = HttpContext.Session.GetString("Cart");
            List<CartItemModel> cart;

            if (string.IsNullOrEmpty(cartjson))
            {

                cart = new List<CartItemModel>();


            }
            else
            {
                cart = JsonSerializer.Deserialize<List<CartItemModel>>(cartjson) ?? new List<CartItemModel>();

            }

            total = cart.Sum(u=>u.Quantity * u.Price);
            if(total >0) total += 3;
            var user = _context.Users.FirstOrDefault(u => u.userName == username);
            if (user == null) return RedirectToAction("Login","Auth");

            var model = new PaymentModel
            {
                location = user.deliveryLocation,
                total = total,
                phonenumber=user.phoneNumber,
                 fullname=user.fullName
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Payment(PaymentModel model)
        {
            var username = HttpContext.Session.GetString("Username");
            if (username == null) return RedirectToAction("Login", "Auth");



            // create order and order details
            // get cart first

            var cartjson = HttpContext.Session.GetString("Cart");
            List<CartItemModel> cart;

            if (string.IsNullOrEmpty(cartjson))
            {

                cart = new List<CartItemModel>();


            }
            else
            {
                cart = JsonSerializer.Deserialize<List<CartItemModel>>(cartjson) ?? new List<CartItemModel>();

            }

            if (!ModelState.IsValid) {
                ViewBag.Total = cart.Sum(u => u.Quantity * u.Price);
                ViewBag.Location = model.location;
                
                return View(model); }

            if(cart == null || cart.Count == 0) return RedirectToAction("Menu","Home");


            var user = _context.Users.FirstOrDefault(u => u.userName == username);
            if (user == null) return RedirectToAction("Login","Auth");


            orderModel order = new orderModel
            {
                UserId = user.Id,
                CreatedAt = DateTime.Now,
                paymentMethod = model.paymentmethod,
                totalPrice = cart.Sum(u=> u.Price * u.Quantity),
                 location = model.location

            };
            _context.Orders.Add(order);
            _context.SaveChanges();
            
            foreach(var item in cart)
            {
                _context.OrderDetails.Add(new OrderDetailsModel { 
                 OrderId = order.Id,
                  itemPrice = item.Price,
                   MenuId=item.MenuID,
                    quantity = item.Quantity
                });
            }
            if (string.IsNullOrEmpty(user.deliveryLocation))
            {
                user.deliveryLocation = model.location;


            } 


            _context.SaveChanges();
            // now save to delivery.
            _context.Deliveries.Add(new deliveryModel
            {
                 OrderId=order.Id,
                  status= Status.Pending,
                   assignedAt = DateTime.Now,
                   
            });



            _context.SaveChanges();

            HttpContext.Session.Remove("Cart");
            return RedirectToAction("UserOrder","Home");
        }
        [HttpGet]
        public IActionResult UserOrder()
        {
            var userstring = HttpContext.Session.GetString("Username");
            if (userstring == null) return RedirectToAction("Login","Auth");

            var user = _context.Users.FirstOrDefault(u => u.userName == userstring);
            if (user == null) return RedirectToAction("Login","Auth");
            //where to list never returns null
            var deliveries = _context.Deliveries
                .Include(d => d.Order) // makes database load the navigation element
                    .ThenInclude(o => o.OrderDetails)
                        .ThenInclude(od => od.Menu)
                .Where(d => d.Order.UserId == user.Id).OrderByDescending(u=>u.Id)
                .ToList();
            // otherwise the context wouldnt load them


            return View(deliveries);
        }

        public IActionResult Profile()
        {

            var user = _context.Users.FirstOrDefault(u=> u.userName== HttpContext.Session.GetString("Username"));
            if (user == null) return RedirectToAction("Login","Auth");



            return View(user);
        }

        public IActionResult Cancelorder(int id)
        {
            var userstring = HttpContext.Session.GetString("Username");
            if (userstring == null) return RedirectToAction("Login","Auth");
            var user = _context.Users.FirstOrDefault(u=>u.userName==userstring);
            if (user == null) return RedirectToAction("Login","Auth");
            var delivery = _context.Deliveries.Include(u=>u.Order).FirstOrDefault(u => u.OrderId == id);
            if (delivery == null || delivery.Order.UserId != user.Id) return RedirectToAction("UserOrder","Home");
            if(delivery.status == Status.Delivered)
            {
                TempData["message"] = "You can't cancel a delivered order!";
                return RedirectToAction("UserOrder", "Home");

            }
            delivery.status = Status.Cancelled;
            delivery.cancelledAt = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("UserOrder","Home");

        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
