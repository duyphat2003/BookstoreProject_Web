using BookstoreProject.Firestore_Database;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }

        //Trang chính
        public IActionResult Index()
        {
            BookstoreProjectDatabase.ConnectToFirestoreDB();
            return View();
        }

        //Trang quản lý tài khoản
        public IActionResult AccountManagement()
        {
            return View();
        }

        //Trang quản lý sách
        public IActionResult BookManagement()
        {
            BookstoreProjectDatabase.ConnectToFirestoreDB();
            BookstoreProjectDatabase.LoadBooks();
            BookstoreProjectDatabase.LoadCopies();
            return View();
        }

        //Trang quản lý phiếu mượn
        public IActionResult LoanManagement()
        {
            return View();
        }

        //Trang quản lý thẻ thư viện
        public IActionResult LibraryCardManagement()
        {
            return View();
        }

        //Trang quản lý bản sao
        public IActionResult BookCopyManagement()
        {
            return View();
        }

        //Trang quản lý thể loại
        public IActionResult GenreManagement()
        {
            return View();
        }
    }
}
