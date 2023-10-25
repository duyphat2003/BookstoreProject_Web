using BookstoreProject.Firestore_Database;
using BookstoreProject.Models;
using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace BookstoreProject.Controllers
{
    //hgjhgjhgjgjkhgjgjkhjgjhb
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            //_auth = new FirebaseAuthProvider(new FirebaseConfig(_apiKey));
        }

        //Trang chính
        public IActionResult Index(string name)
        {
            if (User.Identity.IsAuthenticated)
            {
                BookstoreProjectDatabase.accountInfo = new Account(User.Claims.ElementAt(0).Value, User.Claims.ElementAt(1).Value, User.Claims.ElementAt(2).Value);
                Console.WriteLine("User.Claims.ElementAt(2).ToString(): " + User.Claims.ElementAt(2).Value);
            }          
            if (name != null)
            {
                //Nếu có searchValue lấy ra danh sách book map vs searchValue
                BookstoreProjectDatabase.SearchBook(name);
            }
            else
            {
                //Không có searchValue , load lại Bookdata
                BookstoreProjectDatabase.LoadBooks();
                BookstoreProjectDatabase.LoadGenre();
                BookstoreProjectDatabase.LoadBooksSortedWithCopies();
            }
<<<<<<< HEAD
=======
            BookstoreProjectDatabase.LoadBooks();
            BookstoreProjectDatabase.LoadGenre();
            BookstoreProjectDatabase.LoadBooksSortedWithCopies();
>>>>>>> 34e957cddfb1f7afc8e62ec9b85ea53a9094d993
            return View();
        }

        // Trang danh sách sản phẩm
        public IActionResult BookList()
        {
            return View();
        }

        public IActionResult LoadBooksWithGenre(string nameGenre)
        {
            BookstoreProjectDatabase.LoadBooksWithGenre(nameGenre);
            return RedirectToAction("BookList", "Home");
        }

        [HttpPost]
        public IActionResult LoadBooksWithName(string name)
        {
            BookstoreProjectDatabase.LoadBooksWithKeyword(name);
            return RedirectToAction("BookList", "Home");
        }


        //Trang chi tiết sản phẩm
        public IActionResult Details(string id)
        {
            BookstoreProjectDatabase.ConnectToFirestoreDB();
            BookstoreProjectDatabase.LoadGenre();
            Book book = BookstoreProjectDatabase.LoadContentBookWithId(id);
            ViewBag.meomeo = book;

            return View();
        }

        // Trang giỏ hàng
        public IActionResult Cart()
        {
            BookstoreProjectDatabase.LoadGenre();
            Book book = BookstoreProjectDatabase.LoadContentBookWithId("KNS001");
            ViewBag.meomeo = book;

            return View();
        }

        public IActionResult AddCart()
        {
            return RedirectToAction("Cart", "Home");
        }

        public IActionResult GetMoreProduct(int page=1,int pageSize=5)
        {
            BookstoreProjectDatabase.LoadBooksWithIntitalStatePage(page, pageSize);
            return PartialView("_MoreProductPartial");
        }

        //Trang thông tin người dùng
        public IActionResult UserInfo(string id)
        {
            BookstoreProjectDatabase.LoadLoanWithId(id);   
            return View();
        }

        //Trang lịch sử mượn
        public IActionResult UserLoanHistory()
        {
            return View();
        }
        //Trang thông báo của người dùng 
        public IActionResult UserNofi()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}