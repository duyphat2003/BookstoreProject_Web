namespace BookstoreProject.Firestore_Database
{
    public class BookStoreContext
    {
        public static void InitRequestDB()
        {
            BookstoreProjectDatabase.ConnectToFirestoreDB();
        }

    }
}
