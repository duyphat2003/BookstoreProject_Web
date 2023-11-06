using BookstoreProject.Firestore_Database;
using Firebase.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using BookstoreProject.Dto;
using BookstoreProject.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace BookstoreProject.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

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


        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginDTO loginModel)
        {
            BookstoreProjectDatabase.SearchAccount(loginModel.Account, loginModel.Password);
            if (!string.IsNullOrEmpty(BookstoreProjectDatabase.accountInfo.getAccount()))
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

                if(BookstoreProjectDatabase.accountInfo.getRole().Equals(BookstoreProjectDatabase.SINHVIEN))
                    return RedirectToAction("Index", "Home");
                else
                    return RedirectToAction("Index", "Admin");
            }
            return View();
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            BookstoreProjectDatabase.UpdateAccount(BookstoreProjectDatabase.accountInfo.getAccount(), false);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO dtos)
        {
            bool user= BookstoreProjectDatabase.UpdateAccount(dtos.Account, dtos.ConfirmPassword);
            if (!user)
            {
                throw new ArgumentException("Sai tài khoản");
            }
            else
            {
                return RedirectToAction("SignIn", "User");
            }
        }
        //Trang thông tin người dùng
        public IActionResult UserInfo()
        {
            //string id = BookstoreProjectDatabase.accountInfo.getAccount();
            //BookstoreProjectDatabase.LoadLibraryCardsWithId(id);
            return View();
        }
        //Trang lịch sử mượn
        public IActionResult UserLoanHistory()
        {
            BookstoreProjectDatabase.LoadLoanWithId(BookstoreProjectDatabase.accountInfo.getAccount());
            return View();
        }
        //Trang thông báo của người dùng 
        public IActionResult UserNofi()
        {
            return View();
        }

        //Email Service is not working while using firebase
        //Not sure

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO dtos)
        //{
        //    if (!ModelState.IsValid)
        //        return View(dtos);
        //    var user = await _userManager.FindByEmailAsync(dtos.Email);
        //    if (user == null)
        //        return RedirectToAction(nameof(ForgotPasswordConfirmation));
        //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    var callback = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);
        //    var message = new Message(new string[] { user.Email }, "Reset password token", callback, null);
        //    await _emailSender.SendEmailAsync(message);
        //    return RedirectToAction(nameof(ForgotPasswordConfirmation));
        //}

    }
}
