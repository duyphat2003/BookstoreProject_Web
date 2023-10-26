<<<<<<< HEAD
﻿//using Amazon.IdentityManagement.Model;
using Amazon.IdentityManagement.Model;
using BookstoreProject.Dto;
using BookstoreProject.Firestore_Database;
=======
﻿using BookstoreProject.Firestore_Database;
>>>>>>> 34e957cddfb1f7afc8e62ec9b85ea53a9094d993
using BookstoreProject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
<<<<<<< HEAD
using System.Security.Claims;
=======
using System.Net;
>>>>>>> 34e957cddfb1f7afc8e62ec9b85ea53a9094d993
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

            switch (BookstoreProjectDatabase.accountInfo.getRole())
            {
                case "QuanLy":
                    BookstoreProjectDatabase.GetAccountWithRoles(true);
                    break;
                case "ThuThu":
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
                        if (!BookstoreProjectDatabase.accountInfo.getRole().Equals(BookstoreProjectDatabase.QUANLY) && account[2].Equals('D') && account[3].Equals('H'))
                        {
                            DateTime currentDate = DateTime.Now;
                            DateTime dateDue = currentDate.AddYears(4);
                            if (BookstoreProjectDatabase.AddAccount(new Account(account, account, BookstoreProjectDatabase.SINHVIEN)))
                            {
                                Console.WriteLine("Thêm tài khoản thành công");
                                if (BookstoreProjectDatabase.AddLibraryCard(new LibraryCard(account, "Không tên", dateDue.ToString("dd/MM/yyyy"), true, false)))
                                {
                                    Console.WriteLine("Thêm tài khoản thất bại ADdLib");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Thêm tài khoản thất bại ADdacC");
                            }
                        }
                        else if (BookstoreProjectDatabase.accountInfo.getRole().Equals(BookstoreProjectDatabase.QUANLY) && ((account[2].Equals('T') && account[3].Equals('T')) || (account[2].Equals('T') && account[3].Equals('K'))))
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
                        if (!BookstoreProjectDatabase.accountInfo.getRole().Equals(BookstoreProjectDatabase.QUANLY) && account[2].Equals('D') && account[3].Equals('H') && BookstoreProjectDatabase.FindAccountsWithAccount(account, BookstoreProjectDatabase.SINHVIEN))
                        {
                            BookstoreProjectDatabase.DeleteAccount(account);
                            BookstoreProjectDatabase.DeleteLibraryCard(account);
                            Console.WriteLine("Xóa tài khoản thành công");

                        }
                        else if (BookstoreProjectDatabase.accountInfo.getRole().Equals(BookstoreProjectDatabase.QUANLY) && ((account[2].Equals('T') && account[3].Equals('T') && BookstoreProjectDatabase.FindAccountsWithAccount(account, BookstoreProjectDatabase.THUTHU)) || (account[2].Equals('T') && account[3].Equals('K') && BookstoreProjectDatabase.FindAccountsWithAccount(account, BookstoreProjectDatabase.THUKHO))))
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
                        if (!BookstoreProjectDatabase.accountInfo.getRole().Equals(BookstoreProjectDatabase.QUANLY) && account[2].Equals('D') && account[3].Equals('H') && BookstoreProjectDatabase.FindAccountsWithAccount(account, BookstoreProjectDatabase.SINHVIEN))
                        {
                            BookstoreProjectDatabase.UpdateAccount(account, password, BookstoreProjectDatabase.SINHVIEN);
                            Console.WriteLine("Cập nhật tài khoản thành công");
                        }
                        else if (BookstoreProjectDatabase.accountInfo.getRole().Equals(BookstoreProjectDatabase.QUANLY) && ((account[2].Equals('T') && account[3].Equals('T') && BookstoreProjectDatabase.FindAccountsWithAccount(account, BookstoreProjectDatabase.THUTHU)) || (account[2].Equals('T') && account[3].Equals('K') && BookstoreProjectDatabase.FindAccountsWithAccount(account, BookstoreProjectDatabase.THUKHO))))
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
                case "QuanLy":
                    BookstoreProjectDatabase.GetAccountWithRoles(true);
                    break;
                case "ThuThu":
                    BookstoreProjectDatabase.GetAccountWithRoles(false);
                    break;
            }
            ViewBag.accountList = BookstoreProjectDatabase.accounts;
            return View("AccountManagement");
        }

        Book book = new Book();
        //Trang quản lý sách
<<<<<<< HEAD
        [Authorize(Policy = "Admin", Roles = "ThuKho,QuanLy")]

        public IActionResult BookManagement()
=======
        public IActionResult BookManagement(string nameGenre)
>>>>>>> 34e957cddfb1f7afc8e62ec9b85ea53a9094d993
        {
            if (string.IsNullOrEmpty(nameGenre))
                BookstoreProjectDatabase.LoadBooks();
            else
                BookstoreProjectDatabase.LoadBooksWithGenre(nameGenre);
            BookstoreProjectDatabase.LoadGenre();

            ViewBag.BookContent = book;
            Console.WriteLine("BookManagement book: " + book.getId());
            return View();
        }

        [HttpPost]
        public IActionResult LoadContentBookWithId(string idBook)
        {
            book = BookstoreProjectDatabase.LoadContentBookWithId(idBook);

            BookstoreProjectDatabase.LoadBooks();
            BookstoreProjectDatabase.LoadGenre();
            ViewBag.BookContent = book;
            Console.WriteLine("LoadContentBookWithId book: " + book.getId());
            return RedirectToAction("BookManagement", "Admin");
        }

        [HttpPost]
        public IActionResult ManageBookManagement(string idBook, string name, string genre, string author, string content, string yearPublished, string Publisher, string urlImage, string button)
        {
            Book book1 = new Book();
            switch (button)
            {
                case "add":
                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(author) && !string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(Publisher) && !string.IsNullOrEmpty(urlImage))
                    {
                        List<Book> books = BookstoreProjectDatabase.LoadNameBooksWithGenre(genre);
                        string id = "";
                        switch (genre)
                        {
                            case "Kỹ năng sống":
                                id += "KNS";
                                break;
                            case "Marketing":
                                id += "MKG";
                                break;
                            case "Manga":
                                id += "Manga";
                                break;
                            case "Ngoại ngữ":
                                id += "NN";
                                break;
                            case "Novel":
                                id += "Novel";
                                break;
                        }
                        Console.WriteLine("Sách mới:\n" + id + (books.Count + 1).ToString() + "\n" + name + "\n" + author + "\n" + content + "\n" + yearPublished + "\n" + Publisher + "\n" + urlImage);
                        BookstoreProjectDatabase.AddBook(new Book(id + (books.Count + 1).ToString(), name, author, genre, content, yearPublished, Publisher, urlImage));
                    }
                    break;
                case "delete":
                    BookstoreProjectDatabase.DeleteBook(idBook);
                    break;
                case "update":
                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(author) && !string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(Publisher) && !string.IsNullOrEmpty(urlImage))
                    {
                        BookstoreProjectDatabase.UpdateBook(new Book(idBook, name, author, genre, content, yearPublished, Publisher, urlImage));
                    }
                    break;
            }

            BookstoreProjectDatabase.LoadBooks();
            BookstoreProjectDatabase.LoadGenre();
            ViewBag.BookContent = book1;
            return RedirectToAction("BookManagement", "Admin");
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
                            BookstoreProjectDatabase.AddAccount(new Account(cardId, cardId, BookstoreProjectDatabase.SINHVIEN));
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
            BookstoreProjectDatabase.LoadBooks();
            BookstoreProjectDatabase.LoadCopies();
            ViewBag.copies = new List<Copy>();
            ViewBag.bookId = "";
            ViewBag.currentCopy = new Copy();
            Console.WriteLine("ViewBag.bookId " + ViewBag.bookId);
            return View();
        }

    
        public IActionResult LoadCopy(string bookId)
        {
            BookstoreProjectDatabase.LoadBooks();
            BookstoreProjectDatabase.LoadCopies();
            ViewBag.copies = BookstoreProjectDatabase.LoadCopiesWithBookId(bookId);
            ViewBag.bookId = bookId;
            Console.WriteLine("ViewBag.bookId " + ViewBag.bookId);
            ViewBag.currentCopy = new Copy();
            return View("BookCopyManagement");
        }

      
        public IActionResult LoadInfoCopy(string bookId, string id, string button)
        {
            if(button.Equals("Lấy thông tin")) 
            {
                ViewBag.copies = BookstoreProjectDatabase.LoadCopiesWithBookId(bookId);
                ViewBag.bookId = bookId;
                ViewBag.currentCopy = BookstoreProjectDatabase.LoadCopyInfo(bookId, id);
            }
            else if(button.Equals("Xóa"))
            {
                ViewBag.copies = new List<Copy>();
                ViewBag.bookId = "";
                ViewBag.currentCopy = new Copy();
                BookstoreProjectDatabase.DeleteBookCopy(bookId, id);
            }
            BookstoreProjectDatabase.LoadBooks();
            BookstoreProjectDatabase.LoadCopies();
            return View("BookCopyManagement");
        }


        [HttpPost]
        public IActionResult AddBookCopy(string bookId, string status, string notes)
        {
            List<Copy> copies = BookstoreProjectDatabase.LoadCopiesWithBookId(bookId);
            BookstoreProjectDatabase.AddBookCopy(new Copy((copies.Count + 1).ToString(), bookId, status, notes));
            BookstoreProjectDatabase.LoadBooks();
            BookstoreProjectDatabase.LoadCopies();
            ViewBag.copies = new List<Copy>();
            ViewBag.bookId = "";
            ViewBag.currentCopy = new Copy();
            return View("BookCopyManagement");
        }

        [HttpPost]
        public IActionResult UpdateBookCopy(string id, string bookId, string status, string notes)
        {
            BookstoreProjectDatabase.UpdateBookCopy(new Copy(id, bookId, status, notes));
            BookstoreProjectDatabase.LoadBooks();
            BookstoreProjectDatabase.LoadCopies();
            ViewBag.copies = new List<Copy>();
            ViewBag.bookId = "";
            ViewBag.currentCopy = new Copy();
            return View("BookCopyManagement");
        }

        [HttpPost]
        public IActionResult DeleteBookCopy(string id, string bookId)
        {
            BookstoreProjectDatabase.DeleteBookCopy(bookId, id);
            BookstoreProjectDatabase.LoadBooks();
            BookstoreProjectDatabase.LoadCopies();
            ViewBag.copies = new List<Copy>();
            ViewBag.bookId = "";
            ViewBag.currentCopy = new Copy();
            return View("BookCopyManagement");
        }

        //Trang quản lý thể loại
        [Authorize(Policy = "Admin", Roles = "QuanLy")]
        public IActionResult GenreManagement()
        {
            return View();
        }

        public IActionResult ReportManagement()
        {
            BookstoreProjectDatabase.LoadBooks();
            BookstoreProjectDatabase.LoadCopies();
            BookstoreProjectDatabase.LoadAccounts();
            BookstoreProjectDatabase.LoadLoan();
            BookstoreProjectDatabase.LoadLibraryCards();

            

            return View();
        }
    }
}
