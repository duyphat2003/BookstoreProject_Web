using BookstoreProject.Models;
using Google.Cloud.Firestore;
using Humanizer.Localisation;

namespace BookstoreProject.Firestore_Database
{
    public class BookstoreProjectDatabase
    {
        static string BOOK = "Book"; // Tên bảng
        static string COPY = "Copy"; // Tên bảng
        static string LOAN = "Loan"; // Tên bảng
        static string LIBRARYCARD = "LibraryCard"; // Tên bảng
        static string GENRE = "Genre"; // Tên bảng
        static string ACCOUNT = "Account"; // Tên bảng
        static string projectId = "libraryproject-704cf"; // project id of Bookstore

        static FirestoreDb database;
        static CollectionReference bookCollectionRef, copyCollectionRef, loanCollectionRef, libraryCardCollectionRef, genreCollectionRef, accountCollectionRef; // Các collection trong database Bookstore

        public static void ConnectToFirestoreDB()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"libraryproject-704cf-firebase-adminsdk-trb3v-b601c05ad2.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            // Connect to Database
            database = FirestoreDb.Create(projectId);
            Console.WriteLine("Connected to database successfully!");

            // Connect to Book table
            bookCollectionRef = database.Collection(BOOK);
            Console.WriteLine("Connected to Book table successfully!");

            // Connect to Copy table
            copyCollectionRef = database.Collection(COPY);
            Console.WriteLine("Connected to Copy table successfully!");             

            // Connect to Loan table
            loanCollectionRef = database.Collection(LOAN);
            Console.WriteLine("Connected to Loan table successfully!");

            // Connect to LibraryCard table
            libraryCardCollectionRef = database.Collection(LIBRARYCARD);
            Console.WriteLine("Connected to LibraryCard table successfully!");

            // Connect to Genre table
            genreCollectionRef = database.Collection(GENRE);
            Console.WriteLine("Connected to Genre table successfully!");

            // Connect to Account table
            accountCollectionRef = database.Collection(ACCOUNT);
            Console.WriteLine("Connected to Account table successfully!");
        }

        // Tải thể loại
        public static void LoadGenre()
        {

        }
        // Tải sách
        public static void LoadBooks()
        {
           
        }

        // tải các bản sao của sách
        public static void LoadCopies()
        {

        }

        // Tải sách với bản sao mà sách có tên chứa ký tự tìm kiếm
        public static void SearchBook(string name)
        {

        }

        // Tải sách theo thể loại
        public static void LoadBooksWithGenre(string genreName)
        {
            
        }

        // Login
        public static void SearchAccount(string account, string password)
        {

        }

        // Tải Tài khoản - Manager
        public static void LoadAccounts()
        {

        }


        // tải thẻ sinh viên - Manager
        public static void LoadLibraryCards()
        {

        }

        // Thêm sách - Manager
        public static void AddBook(Book book)
        {

        }

        // cập nhập sách - Manager
        public static void UpdateBook(Book book)
        {

        }

        // xóa sách - Manager
        public static void DeleteBook(string id)
        {

        }

        // Thêm bản sao của sách - Manager
        public static void AddBookCopy(Copy copy)
        {

        }
        // cập nhật bản sao của sách - Manager
        public static void UpdateBookCopy(Copy copy)
        {

        }
        // xóa bản sao của sách - Manager
        public static void DeleteBookCopy(string id)
        {

        }

        // Thêm thẻ thư viện - Manager
        public static void AddLibraryCard(LibraryCard libraryCard)
        {

        }
        // cập nhật thẻ thư viện
        public static void UpdateLibraryCard(LibraryCard libraryCard, string borrowStatus, string useStatus)
        {

        }
        // xóa thẻ thư viện - Manager
        public static void DeleteLibraryCard(string id)
        {

        }
        // Thêm Lần mượn sách- Manager
        public static void AddLoan(Loan loan)
        {

        }
        // cập nhập Lần mượn sách- Manager
        public static void UpdateLoan(Loan loan)
        {

        }
        // xóa Lần mượn sách - Manager
        public static void DeleteLoan(string id)
        {

        }
        // Thêm tài khoản - Manager
        public static void AddAccount(Account account)
        {

        }
        // Cập nhật tài khoản - Manager
        public static void UpdateAccount(Account account)
        {

        }

        // Xóa tài khoản - Manager
        public static void DeleteAccount(string nameAccount)
        {

        }
    }
}
