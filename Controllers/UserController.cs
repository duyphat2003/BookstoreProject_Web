using BookstoreProject.Firestore_Database;
using Firebase.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using BookstoreProject.Dto;

namespace BookstoreProject.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        private FirebaseAuthProvider _auth;
        private static string _apiKey = "AIzaSyCjA_B3IWRegYfuYpe_j_7sPbbksrBbbDI";

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
            _auth = new FirebaseAuthProvider(new FirebaseConfig(_apiKey));
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Registration()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginDTO loginModel)
        {
            BookstoreProjectDatabase.SearchAccount(loginModel.Account, loginModel.Password);
            if (BookstoreProjectDatabase.accountInfo != null)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, BookstoreProjectDatabase.accountInfo.getAccount()),
                        new Claim(ClaimTypes.SerialNumber, BookstoreProjectDatabase.accountInfo.getPassword()),
                        new Claim(ClaimTypes.Role, BookstoreProjectDatabase.accountInfo.getRole()),
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties { };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity),authProperties);

                if(BookstoreProjectDatabase.accountInfo.getRole().Equals("Sinh viên"))
                    return RedirectToAction("Index", "Home");
                else
                    return RedirectToAction("Index", "Admin");
            }
            return View();
        }
        public async Task<ActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            BookstoreProjectDatabase.UpdateAccount(BookstoreProjectDatabase.accountInfo.getAccount(), false);
            return RedirectToAction("Index", "Home");
        }

        //[HttpPost]
        //public async Task<IActionResult> Registration(SignUpDTO loginModel)
        //{
        //    BookstoreProjectDatabase.SearchAccount(loginModel.Account, loginModel.Password);
        //    if (BookstoreProjectDatabase.accountInfo != null)
        //    {
        //        throw new Exception("This account is already created.");
        //    }

        //    BookstoreProjectDatabase.AddAccount(loginModel);
        //    var claims = new List<Claim>
        //            {
        //                new Claim(ClaimTypes.Name, BookstoreProjectDatabase.accountInfo.getAccount()),
        //                new Claim(ClaimTypes.Role, BookstoreProjectDatabase.accountInfo.getRole()),
        //            };
        //    var claimsIdentity = new ClaimsIdentity(
        //            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //    var authProperties = new AuthenticationProperties { };

        //    await HttpContext.SignInAsync(
        //    CookieAuthenticationDefaults.AuthenticationScheme,
        //    new ClaimsPrincipal(claimsIdentity),
        //     authProperties);
        //    return RedirectToAction("Index", "Home");
        //}

    }
}
