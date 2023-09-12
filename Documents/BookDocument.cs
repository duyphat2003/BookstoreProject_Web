using Google.Cloud.Firestore;

namespace BookstoreProject.Documents
{
    [FirestoreData]



    public class BookDocument
    {
        [FirestoreDocumentId]
        public string Id { get; set; }
        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public string Publisher { get; set; }
        [FirestoreProperty]
        public string Author { get; set; }
        [FirestoreProperty]
        public string Genre { get; set; }
        [FirestoreProperty]
        public List<string> Content { get; set; }
        [FirestoreProperty]
        public string Url { get; set; }
        [FirestoreProperty]
        public int YearPublish { get; set; }

    }
}
