using Microsoft.AspNetCore.Mvc;
using SaqrResturant.Data;
using SaqrResturant.Models;


// TODO: 1.Make change password a thing.


namespace SaqrResturant.Controllers
{
    public class AuthController : Controller
    {
        //context querier
        private readonly ResturantContext _context;
        //context querier creation
        public AuthController(ResturantContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Username") == null)
                return View(new LoginModel());
            else { return RedirectToAction("Index","Home"); }
        }
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            //check if the fields are wrong
            if(!ModelState.IsValid) return View("Login",model);


            var user = _context.Users.FirstOrDefault(u => u.userName == model.userName);
            // check if user information is correct
            // DOESN'T crash becuase if first value is true, will not check the second!
            if (user == null || user.password != model.password) {
                ViewBag.message = "Wrong credentials.";
                ViewBag.color = "red";
                return View(model);
            }

            


            HttpContext.Session.SetString("Username", user.userName);
            HttpContext.Session.SetString("Userrole", user.role.ToString());


            return RedirectToAction("Index","Home");
        }
        [HttpGet]
        public IActionResult Signup()
        {
            if (HttpContext.Session.GetString("Username") == null) { return View(new SignupModel()); }

            return RedirectToAction("Index","Home");
        }
        [HttpPost]
        public IActionResult Signup(SignupModel model)
        {
            if (!ModelState.IsValid) { return View(model); }
            if (model.password != model.Confpassword) { ViewBag.message = "Passwords don't match!"; return View(model); }
            //make sure if our full name has any numbers

            if (model.fullName.Any(char.IsDigit)) {
                ViewBag.message = "Full name cant contain any numbers!";
                return View(model);

            }

            if (_context.Users.FirstOrDefault(u => u.userName == model.userName) != null) {
                ViewBag.message = "User already exists!";
                return View(model);

            }
            UserModel user = new UserModel {

                fullName = model.fullName,
                deliveryLocation = model.deliveryLocation,
                password = model.password,
                phoneNumber = model.phoneNumber,
                role = Role.User,
                userName = model.userName };

            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["message"] = "Account creation successful, Please sign in";

            return RedirectToAction("Login","Auth");   

        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("Userrole");


            return RedirectToAction("Index","Home");
        }

        public IActionResult ChangePassword()
        {  if (HttpContext.Session.GetString("Username") == null) { return RedirectToAction("Login", "Auth"); }
            System.Diagnostics.Debug.WriteLine("GET HIT");
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangepasswordModel model)
        {
            if (!ModelState.IsValid) { return View(model); }
            var user = _context.Users.FirstOrDefault(u=>u.userName== HttpContext.Session.GetString("Username"));
            if (user == null) return RedirectToAction("Login","Auth");
            if (user.password != model.currentpassword)
            {
                System.Diagnostics.Debug.WriteLine("POST HIT");
                TempData["message"] = "Current password is incorrect";
                return Redirect("/Auth/ChangePassword");
            }
            user.password = model.newpassword;
            _context.SaveChanges();
            TempData["message"] = "Password updated successfully";
            return RedirectToAction("Profile","Home");
        }


    }
}
