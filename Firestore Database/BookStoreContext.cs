namespace BookstoreProject.Firestore_Database
{
    public class BookStoreContext
    {
        public static void InitRequestDB()
        {
            BookstoreProjectDatabase.ConnectToFirestoreDB();
            BookstoreProjectDatabase.LoadBooks();
            BookstoreProjectDatabase.LoadCopies();
            BookstoreProjectDatabase.LoadGenre();
            BookstoreProjectDatabase.LoadBooksSortedWithCopies();
        }

    }
}
