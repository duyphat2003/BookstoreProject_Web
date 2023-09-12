namespace BookstoreProject.Models
{
    public class Copy
    {
        string id;
        string bookId;
        string status;
        string notes;

        public Copy(string id, string bookId, string status, string notes)
        {
            this.id = id;
            this.bookId = bookId;
            this.status = status;
            this.notes = notes;
        }

        public string getId()
        {
            return id;
        }

        public void setId(string id)
        {
            this.id = id;
        }

        public string getBookId()
        {
            return bookId;
        }

        public void setBookId(string bookId)
        {
            this.bookId = bookId;
        }

        public string getStatus()
        {
            return status;
        }

        public void setStatus(string status)
        {
            this.status = status;
        }

        public string getNotes()
        {
            return notes;
        }

        public void setNotes(string notes)
        {
            this.notes = notes;
        }
    }
}
