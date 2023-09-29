using BookstoreProject.Firestore_Database;
using BookstoreProject.Models;
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
            BookstoreProjectDatabase.LoadBooks();
            return View();
        }

        //Trang quản lý phiếu mượn
        public IActionResult LoanManagement()
        {
            BookstoreProjectDatabase.LoadBooks();
            BookstoreProjectDatabase.LoadLoan();
            ViewBag.loanList = BookstoreProjectDatabase.loans;
            return View();
        }

        public IActionResult AddLoan(string copyId, string bookId, string cardId, string button)
        {
            if(button == "addBtn")
            {
                DateTime currentDate = DateTime.Now;
                DateTime dateDue = currentDate.AddDays(15);
                BookstoreProjectDatabase.AddLoan(new Loan(bookId, cardId, copyId, currentDate.ToString("dd/MM/yyyy"), dateDue.ToString("dd/MM/yyyy")));
            }
            BookstoreProjectDatabase.LoadBooks();
            BookstoreProjectDatabase.LoadLoan();
            ViewBag.loanList = BookstoreProjectDatabase.loans;
            return View("LoanManagement");
        }
        //Trang quản lý thẻ thư viện
        public IActionResult LibraryCardManagement()
        {
            BookstoreProjectDatabase.LoadLibraryCards();
            ViewBag.libraryCardList = BookstoreProjectDatabase.libraryCards;
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
