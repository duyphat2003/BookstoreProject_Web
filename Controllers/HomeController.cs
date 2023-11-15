using BookstoreProject.Firestore_Database;
using BookstoreProject.Models;
using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace BookstoreProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        //Trang chính
        public IActionResult Index(string name)
        {
            if (User.Identity.IsAuthenticated)
            {
                BookstoreProjectDatabase.accountInfo = new Account(User.Claims.ElementAt(0).Value, User.Claims.ElementAt(1).Value, User.Claims.ElementAt(2).Value);
                BookstoreProjectDatabase.libraryCard = BookstoreProjectDatabase.LoadLibraryCardWithId(BookstoreProjectDatabase.accountInfo.getAccount());
                Console.WriteLine("BookstoreProjectDatabase.libraryCard.getId(): " + BookstoreProjectDatabase.libraryCard.getId());
                Console.WriteLine("User.Claims.ElementAt(2).ToString(): " + User.Claims.ElementAt(2).Value);
            }
            else
            {
                BookstoreProjectDatabase.accountInfo = new Account("", "", "");
                Console.WriteLine("BookstoreProjectDatabase.accountInfo.getAccount(): " + BookstoreProjectDatabase.accountInfo.getAccount());

            }


            BookstoreProjectDatabase.LoadBooks();
            BookstoreProjectDatabase.LoadGenre();
            BookstoreProjectDatabase.LoadBooksSortedWithCopies();
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

        public IActionResult LoadBooksWithYearPublished(bool isACS)
        {
            BookstoreProjectDatabase.LoadBooksWithYearPublished(isACS);
            return RedirectToAction("BookList", "Home");
        }

        public IActionResult LoadBooksWithAuthor(string author)
        {
            BookstoreProjectDatabase.LoadBooksWithAuthor(author);
            return RedirectToAction("BookList", "Home");
        }


        [HttpPost]
        public IActionResult LoadBooksWithName(string name)
        {
            BookstoreProjectDatabase.LoadBooksWithKeyword(name);
            return RedirectToAction("BookList", "Home");
        }
        public IActionResult LoadBooksAll()
        {
            BookstoreProjectDatabase.LoadBooks();
            BookstoreProjectDatabase.LoadCopies();
            BookstoreProjectDatabase.LoadGenre();
            return RedirectToAction("BookList", "Home");
        }


        //Trang chi tiết sản phẩm
        public IActionResult Details(string id)
        {
            BookstoreProjectDatabase.ConnectToFirestoreDB();
            BookstoreProjectDatabase.LoadGenre();
            Book book = BookstoreProjectDatabase.LoadContentBookWithId(id);
            Console.WriteLine("Book:\n" + book.getContent());
            ViewBag.book = book;

            return View();
        }
        // Trang giỏ hàng
        public IActionResult Cart()
        {
            BookstoreProjectDatabase.LoadGenre();
            
            ViewBag.bookcart = CartList.books;
            ViewBag.copiescart = CartList.copies1;
            return View();
        }
        [HttpPost]
        public IActionResult AddCart(string id, string title, string author, string genre, string content, string yearPublished, string publisher, string urlImage)
        {
            if (BookstoreProjectDatabase.accountInfo.getAccount() == "")
            {
                Console.WriteLine("Phải đăng nhập!");
                return RedirectToAction("Details", "Home");
            }

            if (CartList.books.Count() < 3) 
            {
                Book book = new Book(id, title, author, genre, content, yearPublished, publisher, urlImage);
                List<Copy> copies = BookstoreProjectDatabase.LoadCopiesWithBookId(book.getId(), "Còn");
                if(copies.Count() > 0)
                {
                    CartList.books.Add(book);
                    CartList.copies1.Add(copies.ElementAt(0));
                    Console.WriteLine("Thêm giỏ hàng thành công!: " + "books: " + CartList.books.Count);
                    Console.WriteLine("Thêm giỏ hàng thành công!: " + "copies1: " + CartList.copies1.Count);
                    ViewBag.bookcart = CartList.books;                                   
                    ViewBag.copiescart = CartList.copies1;
                    return RedirectToAction("Cart", "Home");
                }
                else
                {
                    Console.WriteLine("Thêm giỏ hàng không thành công!");
                    return RedirectToAction("Details", "Home");
                }
            }
            else
            {
                Console.WriteLine("Số lượng trong giỏ hàng lớn hơn 3!");
                return RedirectToAction("Details", "Home");
            }
        }

        public IActionResult RemoveItem(Book book, Copy copy)
        {
            CartList.books.Remove(book);
            CartList.copies1.Remove(copy);
            ViewBag.bookcart = CartList.books;
            ViewBag.copiescart = CartList.copies1;
            return RedirectToAction("Cart", "Home");
        }

        //public IActionResult AddOrRemoveItem(Copy copy)
        //{
        //    bool isExist = false;
        //    foreach(Copy copy1 in CartList.copyChosen)
        //    {
        //        if(copy1 == copy)
        //        {
        //            isExist = true;
        //            break;
        //        }
        //    }

        //    if(!isExist)
        //        CartList.copyChosen.Add(copy);
        //    else
        //        CartList.copyChosen.Remove(copy);

        //    ViewBag.bookcart = CartList.books;
        //    ViewBag.copiescart = CartList.copies1;
        //    return RedirectToAction("Cart", "Home");
        //}

        public IActionResult Borrow()
        {
            if (BookstoreProjectDatabase.accountInfo.getAccount() == "")
            {
                return RedirectToAction("Index", "Home");
            }
            if (CartList.books.Count() < 3)
            {
                foreach(Copy copy in CartList.copyChosen)
                {
                    DateTime dateTime = DateTime.Now;
                    string currentDate = dateTime.ToString("dd/MM/yyyy");
                    dateTime.AddDays(15);
                    string dueDate = dateTime.ToString("dd/MM/yyyy");

                    BookstoreProjectDatabase.AddLoan(new Loan(copy.getBookId(), BookstoreProjectDatabase.libraryCard.getId(), copy.getId(), currentDate, dueDate));
                    BookstoreProjectDatabase.UpdateBookCopy(copy);
                }

                CartList.books = new List<Book>();
                CartList.copies1 = new List<Copy>();
                BookstoreProjectDatabase.libraryCard.setBorrowStatus(true);
                BookstoreProjectDatabase.UpdateLibraryCard(BookstoreProjectDatabase.libraryCard);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.bookcart = CartList.books;
                ViewBag.copiescart = CartList.copies1;
                return RedirectToAction("Cart", "Home");
            }
        }

        public IActionResult GetMoreProduct(int page=1,int pageSize=5)
        {
            BookstoreProjectDatabase.LoadBooksWithIntitalStatePage(page, pageSize);
            return PartialView("_MoreProductPartial");
        }

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class CartList
    {
        public static List<Book> books;
        public static List<Copy> copies1;
        public static List<Copy> copyChosen;

        public static void CreateCart()
        {
            books = new List<Book>();
            copies1 = new List<Copy>();
            copyChosen = new List<Copy>();
        }
    }
}