using Google.Cloud.Firestore;

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

        public static void LoadBooks()
        {
            
        }
    }
}
