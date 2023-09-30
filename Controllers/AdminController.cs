using BookstoreProject.Firestore_Database;
using BookstoreProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

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
            if (cardId != "" && bookId != "" && cardId != "")
            {
                if (button == "addBtn")
                {
                    DateTime currentDate = DateTime.Now;
                    DateTime dateDue = currentDate.AddDays(15);
                    if (bookId != "" && cardId != "" && copyId != "")
                        BookstoreProjectDatabase.AddLoan(new Loan(bookId, cardId, copyId, currentDate.ToString("dd/MM/yyyy"), dateDue.ToString("dd/MM/yyyy")));
                }
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

        public IActionResult AddLibraryCard(string cardId, string nameStudent, string button)
        {
            Console.OutputEncoding = Encoding.Unicode;
            if (cardId != "" && nameStudent != "")
            {
                switch (button)
                {
                    case "add":
                        DateTime currentDate = DateTime.Now;
                        DateTime dateDue = currentDate.AddYears(4);
                        if (BookstoreProjectDatabase.AddLibraryCard(new LibraryCard(cardId, nameStudent, dateDue.ToString("dd/MM/yyyy"), true, false)))
                        {
                            BookstoreProjectDatabase.AddAccount(new Account(cardId, cardId, "Sinh viên"));
                            Console.WriteLine("Thêm thẻ thành công");
                        }
                        else
                        {
                            Console.WriteLine("Thêm thẻ thất bại");
                        }
                        break;
                    case "delete":
                        break;
                    case "update":
                        break;
                }
            }
            Console.WriteLine("Tải lại dữ liệu");
            BookstoreProjectDatabase.LoadLibraryCards();
            ViewBag.libraryCardList = BookstoreProjectDatabase.libraryCards;
            return View("LibraryCardManagement");
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
