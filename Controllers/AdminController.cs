//using Amazon.IdentityManagement.Model;
using Amazon.IdentityManagement.Model;
using BookstoreProject.Dto;
using BookstoreProject.Firestore_Database;
using BookstoreProject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace BookstoreProject.Controllers
{
    [Authorize(Policy = "Admin")]
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
        [Authorize(Policy = "Admin",Roles = "ThuThu,QuanLy")]
        public IActionResult AccountManagement()
        {
            switch(BookstoreProjectDatabase.accountInfo.getRole())
            {
                case "Quản lý":
                    BookstoreProjectDatabase.GetAccountWithRoles(true);
                    break;
                case "Thủ thư":
                    BookstoreProjectDatabase.GetAccountWithRoles(false);
                    break;
            }
            ViewBag.accountList = BookstoreProjectDatabase.accounts;
            return View();
        }

        [HttpPost]
        public IActionResult ClickButtonAccount(string account, string password, string role, string button)
        {
            switch (button)
            {
                case "add":
                    if (!string.IsNullOrEmpty(account) && account.Length == 10)
                    {
                        if (!BookstoreProjectDatabase.accountInfo.getRole().Equals("Quản lý") && account[2].Equals('D') && account[3].Equals('H'))
                        {
                            DateTime currentDate = DateTime.Now;
                            DateTime dateDue = currentDate.AddYears(4);
                            if (BookstoreProjectDatabase.AddAccount(new Account(account, account, "Sinh viên")))
                            {
                                Console.WriteLine("Thêm tài khoản thành công");
                                if(BookstoreProjectDatabase.AddLibraryCard(new LibraryCard(account, "Không tên", dateDue.ToString("dd/MM/yyyy"), true, false)))
                                {
                                    Console.WriteLine("Thêm tài khoản thất bại ADdLib");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Thêm tài khoản thất bại ADdacC");
                            }
                        }
                        else if (BookstoreProjectDatabase.accountInfo.getRole().Equals("Quản lý") && ((account[2].Equals('T') && account[3].Equals('T')) || (account[2].Equals('T') && account[3].Equals('K'))))
                        {
                            BookstoreProjectDatabase.AddAccount(new Account(account, password, role));
                            Console.WriteLine("Thêm tài khoản thành công");
                        }
                        else
                        {
                            Console.WriteLine("Thêm tài khoản thất bại :" + account[2] + account[3]);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Thêm tài khoản thất bại length: " + account.Length);
                    }
                    break;
                case "delete":
                    if (!string.IsNullOrEmpty(account) && account.Length == 10)
                    {
                        if (!BookstoreProjectDatabase.accountInfo.getRole().Equals("Quản lý") && account[2].Equals('D') && account[3].Equals('H') && BookstoreProjectDatabase.FindAccountsWithAccount(account, "Sinh viên"))
                        {
                            BookstoreProjectDatabase.DeleteAccount(account);
                            BookstoreProjectDatabase.DeleteLibraryCard(account);
                            Console.WriteLine("Xóa tài khoản thành công");

                        }
                        else if (BookstoreProjectDatabase.accountInfo.getRole().Equals("Quản lý") && ((account[2].Equals('T') && account[3].Equals('T') && BookstoreProjectDatabase.FindAccountsWithAccount(account, "Thủ thư")) || (account[2].Equals('T') && account[3].Equals('K') && BookstoreProjectDatabase.FindAccountsWithAccount(account, "Thủ kho"))))
                        {
                            BookstoreProjectDatabase.DeleteAccount(account);
                            Console.WriteLine("Xóa tài khoản thành công");
                        }
                        else
                        {
                            Console.WriteLine("Xóa tài khoản thất bại");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Xóa tài khoản thất bại");
                    }
                    break;
                case "update":
                    if (!string.IsNullOrEmpty(account) && account.Length == 10 && !string.IsNullOrEmpty(password))
                    {
                        if (!BookstoreProjectDatabase.accountInfo.getRole().Equals("Quản lý") && account[2].Equals('D') && account[3].Equals('H') && BookstoreProjectDatabase.FindAccountsWithAccount(account, "Sinh viên"))
                        {
                            BookstoreProjectDatabase.UpdateAccount(account, password, "Sinh viên");
                            Console.WriteLine("Cập nhật tài khoản thành công");
                        }
                        else if (BookstoreProjectDatabase.accountInfo.getRole().Equals("Quản lý") && ((account[2].Equals('T') && account[3].Equals('T') && BookstoreProjectDatabase.FindAccountsWithAccount(account, "Thủ thư")) || (account[2].Equals('T') && account[3].Equals('K') && BookstoreProjectDatabase.FindAccountsWithAccount(account, "Thủ kho"))))
                        {
                            BookstoreProjectDatabase.UpdateAccount(account, password, role);
                            Console.WriteLine("Cập nhật tài khoản thành công");
                        }
                        else
                        {
                            Console.WriteLine("Cập nhật tài khoản thất bại");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Cập nhật tài khoản thất bại");
                    }
                    break;
            }

            switch (BookstoreProjectDatabase.accountInfo.getRole())
            {
                case "Quản lý":
                    BookstoreProjectDatabase.GetAccountWithRoles(true);
                    break;
                case "Thủ thư":
                    BookstoreProjectDatabase.GetAccountWithRoles(false);
                    break;
            }
            ViewBag.accountList = BookstoreProjectDatabase.accounts;
            return View("AccountManagement");
        }

        //Trang quản lý sách
        [Authorize(Policy = "Admin", Roles = "ThuKho,QuanLy")]

        public IActionResult BookManagement()
        {
            BookstoreProjectDatabase.LoadBooks();
            return View();
        }

        //Trang quản lý phiếu mượn
        [Authorize(Policy = "Admin", Roles = "ThuThu,QuanLy")]
        public IActionResult LoanManagement()
        {
            BookstoreProjectDatabase.LoadBooks();
            BookstoreProjectDatabase.LoadLoan();
            ViewBag.loanList = BookstoreProjectDatabase.loans;
            return View();
        }

        [Authorize(Policy = "Admin", Roles = "ThuThu,QuanLy")]
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
        [Authorize(Policy = "Admin", Roles = "ThuThu,QuanLy")]
        public IActionResult LibraryCardManagement()
        {
            BookstoreProjectDatabase.LoadLibraryCards();
            ViewBag.libraryCardList = BookstoreProjectDatabase.libraryCards;
            return View();
        }

        [Authorize(Policy = "Admin", Roles = "ThuThu,QuanLy")]
        public IActionResult AddLibraryCard(string cardId, string nameStudent, string button, bool status, bool borrow)
        {
            Console.OutputEncoding = Encoding.Unicode;

                switch (button)
                {
                    case "add":
                        if (cardId != "" && nameStudent != "")
                        {
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
                        }
                        break;
                    case "delete":
                        if (cardId != "")
                            BookstoreProjectDatabase.DeleteLibraryCard(cardId);
                            BookstoreProjectDatabase.DeleteAccount(cardId);
                        break;
                    case "update":
                        if (cardId != "" && nameStudent != "")
                            BookstoreProjectDatabase.UpdateLibraryCard(new LibraryCard(cardId, nameStudent, status, borrow));
                        break;
            }
            Console.WriteLine("Tải lại dữ liệu");
            BookstoreProjectDatabase.LoadLibraryCards();
            ViewBag.libraryCardList = BookstoreProjectDatabase.libraryCards;
            return View("LibraryCardManagement");
        }

        //Quản lí tài khoản => (thêm tài khoản nhân viên)
        //[HttpPost]
        //public async Task<IActionResult> Registration(SignUpDTO dtos)
        //{
        //    BookstoreProjectDatabase.SearchAccount(dtos.Account, dtos.Password);
        //    if (BookstoreProjectDatabase.accountInfo != null)
        //    {
        //        throw new Exception("This account is already created.");
        //    }
        //    Account account = new Account(dtos.Account, dtos.Password, dtos.Role);
        //    BookstoreProjectDatabase.AddAccount(account);
        //    return RedirectToAction("AccountManagement", "Admin");
        //}

        //Trang quản lý bản sao
        [Authorize(Policy = "Admin", Roles = "ThuKho,QuanLy")]
        public IActionResult BookCopyManagement()
        {
            return View();
        }

        //Trang quản lý thể loại
        [Authorize(Policy = "Admin", Roles = "QuanLy")]
        public IActionResult GenreManagement()
        {
            return View();
        }
    }
}
