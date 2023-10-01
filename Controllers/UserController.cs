using BookstoreProject.Firestore_Database;
using BookstoreProject.Models;
using Firebase.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

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
        public async Task<IActionResult> SignIn(LoginModel loginModel)
        {
            //try
            //{

            //    //log in an existing user
            //    //var fbAuthLink = await _auth
            //    //                .SignInWithEmailAndPasswordAsync(loginModel.Email, loginModel.Password);
            //    //string token = fbAuthLink.FirebaseToken;
            //    ////save the token to a session variable
            //    //if (token != null)
            //    //{
            //    //    HttpContext.Session.SetString("_UserToken", token);
            //    //    return RedirectToAction("index","home");
            //    //}

            //}
            //catch (FirebaseAuthException ex)
            //{
            //    var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
            //    ModelState.AddModelError(String.Empty, firebaseEx.error.message);
            //    return View(loginModel);
            //}
            BookstoreProjectDatabase.SearchAccount(loginModel.Account, loginModel.Password);
            if (BookstoreProjectDatabase.accountInfo != null)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, BookstoreProjectDatabase.accountInfo.getAccount()),
                        new Claim(ClaimTypes.Role, BookstoreProjectDatabase.accountInfo.getRole()),
                    };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties { };

                await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                 authProperties);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        public async Task<ActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Registration(LoginModel loginModel)
        {
            //try
            //{
            //    //create the user
            //    await _auth.CreateUserWithEmailAndPasswordAsync(loginModel.Account, loginModel.Password);
            //    //log in the new user
            //    var fbAuthLink = await _auth
            //                    .SignInWithEmailAndPasswordAsync(loginModel.Account, loginModel.Password);
            //    string token = fbAuthLink.FirebaseToken;
            //    //saving the token in a session variable
            //    if (token != null)
            //    {
            //        HttpContext.Session.SetString("_UserToken", token);

            //        return RedirectToAction("index", "home");
            //    }
            //}
            //catch (FirebaseAuthException ex)
            //{
            //    var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
            //    ModelState.AddModelError(String.Empty, firebaseEx.error.message);
            //    return View(loginModel);
            //}
            BookstoreProjectDatabase.SearchAccount(loginModel.Account, loginModel.Password);
            if (BookstoreProjectDatabase.accountInfo != null)
            {
                throw new Exception("This account is already created.");
            }

            BookstoreProjectDatabase.AddAccount(loginModel);
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, BookstoreProjectDatabase.accountInfo.getAccount()),
                        new Claim(ClaimTypes.Role, BookstoreProjectDatabase.accountInfo.getRole()),
                    };
            var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties { };

            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
             authProperties);
            return RedirectToAction("Index", "Home");
        }

    }
}
