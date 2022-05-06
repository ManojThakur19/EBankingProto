 using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using DbHelper;
using MigrationHelper.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace EbankingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataHelper _Helper;

        public HomeController(IDataHelper helper)
        {
            _Helper = helper;
        }


        public ActionResult Login()
        {
            var model = new Login();
            return View(model);
        }


        [HttpPost]
        public async Task<ActionResult> Login(Login model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Result = "Enter all the required fields in current format.";
                return View(model);
            }

            var user = await _Helper.GetLoginUserAsync(model.Username, model.PinNumber);

            if (user == null)
            {
                ViewBag.Result = "Invalid username or pin number";
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name,ClaimTypes.Role);
                identity.AddClaim(new Claim("Id", user.ID.ToString()));

                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = false });


                return RedirectToAction("Index", "Home", new {Area ="Main" });
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Register(Register model)
        {
            await _Helper.RegisterUser(
                model.Username,
                model.Firstname,
                model.Lastname,
                model.PinNumber,
                model.AccNum,
                model.AccountType
                );

            return RedirectToAction("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
