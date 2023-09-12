using BookstoreProject.Documents;
using BookstoreProject.Models;
using Google.Cloud.Firestore;

namespace BookstoreProject.Repository
{
    public class BookRepository:IFirestoreService
    {
        private readonly FirestoreDb _firestoreDb;
        private const string _collectionName = "Book";

        public BookRepository(FirestoreDb firestoreDb)
        {
            _firestoreDb = firestoreDb;
        }
        public async Task<List<Book>> GetAll()
        {
            var collection = _firestoreDb.Collection(_collectionName);
            var snapshot = await collection.GetSnapshotAsync();

            var bookDocuments = snapshot.Documents.Select(s => s.ConvertTo<BookDocument>()).ToList();
            return bookDocuments.Select(ConvertDocumentToModel).ToList();
        }

        private static Book ConvertDocumentToModel(BookDocument bookDocument)
        {
            return new Book
            {
                Id = bookDocument.Id,
                Name = bookDocument.Name,
                Author = bookDocument.Author,
                Genre = bookDocument.Genre,
                Publisher= bookDocument.Publisher,
                Url = bookDocument.Url,
                Content = bookDocument.Content
            };
        }
        private static BookDocument ConvertModelToDocument(Book book)
        {
            return new BookDocument
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                Genre = book.Genre,
                Publisher= book.Publisher,
                Url = book.Url,
                Content = book.Content
            };
        }

        public async Task AddAsync(Book book)
        {
            var collection = _firestoreDb.Collection(_collectionName);
            var bookDocument = ConvertModelToDocument(book);

            await collection.AddAsync(bookDocument);
        }
    }
}
