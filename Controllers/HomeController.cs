using BookstoreProject.Firestore_Database;
using BookstoreProject.Models;
using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.Serialization;

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

        List<Book> books;
        List<Copy> copies1;
        List<Copy> copyChosen;

        //Trang chính
        public IActionResult Index(string name)
        {
            if (User.Identity.IsAuthenticated)
            {
                BookstoreProjectDatabase.accountInfo = new Account(User.Claims.ElementAt(0).Value, User.Claims.ElementAt(1).Value, User.Claims.ElementAt(2).Value);
                Console.WriteLine("User.Claims.ElementAt(2).ToString(): " + User.Claims.ElementAt(2).Value);
            }
            else
            {
                BookstoreProjectDatabase.accountInfo = new Account("", "", "");
                Console.WriteLine("accountInfo: " + BookstoreProjectDatabase.accountInfo.getAccount());
            }

            if (!IsIn)
            {
                IsIn = true;
                books = new List<Book>();
                copies1 = new List<Copy>();
                copyChosen = new List<Copy>();
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
            Console.WriteLine("Book:\n" + book.getContent());
            ViewBag.book = book;

            return View();
        }

        bool IsIn = false;
        // Trang giỏ hàng
        public IActionResult Cart()
        {
            BookstoreProjectDatabase.LoadGenre();
            
            ViewBag.bookcart = books;
            ViewBag.copiescart = copies1;
            ViewBag.copieschosencart = copyChosen;
            return View();
        }


        public IActionResult AddCart(Book book)
        {
            if(BookstoreProjectDatabase.accountInfo.getAccount() == "")
                return View("Details");

            if (books.Count() < 3) 
            {
                List<Copy> copies = BookstoreProjectDatabase.LoadCopiesWithBookId(book.getId(), "Còn");
                if(copies.Count() > 0)
                {
                    books.Add(book);
                    copies1.Add(copies1.ElementAt(0));
                    return RedirectToAction("Cart", "Home");
                }
                else
                {
                    return View("Details");
                }
            }
            else
            {
                return View("Details");
            }
        }

        public IActionResult RemoveItem(Book book, Copy copy)
        {
            books.Remove(book);
            copies1.Remove(copy);
            return View("Cart");
        }

        public IActionResult AddOrRemoveItem(Copy copy)
        {
            bool isExist = false;
            foreach(Copy copy1 in copyChosen)
            {
                if(copy1 == copy)
                {
                    isExist = true;
                    break;
                }
            }

            if(!isExist)
                copyChosen.Add(copy);
            else
                copyChosen.Remove(copy);

            return View("Cart");
        }

        public IActionResult Borrow()
        {
            if (BookstoreProjectDatabase.accountInfo.getAccount() == "")
                return View("Cart");
            if (copyChosen.Count() < 3)
            {
                foreach(Copy copy in copyChosen)
                {
                    DateTime dateTime = DateTime.Now;
                    string currentDate = dateTime.ToString("dd/MM/yyyy");
                    dateTime.AddDays(15);
                    string dueDate = dateTime.ToString("dd/MM/yyyy");

                    BookstoreProjectDatabase.AddLoan(new Loan(copy.getBookId(), BookstoreProjectDatabase.libraryCard.getId(), copy.getId(), currentDate, dueDate));
                    BookstoreProjectDatabase.UpdateBookCopy(copy);
                    foreach(Book book in books)
                    {
                        if(book.getId() == copy.getBookId())
                        {
                            books.Remove(book);
                            break;
                        }
                    }

                    foreach (Copy copy1 in copies1)
                    {
                        if (copy1.getId() == copy.getId())
                        {
                            copies1.Remove(copy1);
                            break;
                        }
                    }
                }

                copyChosen = new List<Copy>();
                
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("Cart");
            }
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