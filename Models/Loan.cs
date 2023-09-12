namespace BookstoreProject.Models
{
    public class Loan
    {
        string bookId;
        string cardId;
        string copyId;
        string dateLoaned;
        string dateDue;

        public Loan(string bookId, string cardId, string copyId, string dateLoaned, string dateDue)
        {
            this.bookId = bookId;
            this.cardId = cardId;
            this.copyId = copyId;
            this.dateLoaned = dateLoaned;
            this.dateDue = dateDue;
        }

        public string getBookId()
        {
            return bookId;
        }

        public void setBookId(string bookId)
        {
            this.bookId = bookId;
        }

        public string getCardId()
        {
            return cardId;
        }

        public void setCardId(string cardId)
        {
            this.cardId = cardId;
        }

        public string getCopyId()
        {
            return copyId;
        }

        public void setCopyId(string copyId)
        {
            this.copyId = copyId;
        }

        public string getDateLoaned()
        {
            return dateLoaned;
        }

        public void setDateLoaned(string dateLoaned)
        {
            this.dateLoaned = dateLoaned;
        }

        public string getDateDue()
        {
            return dateDue;
        }

        public void setDateDue(string dateDue)
        {
            this.dateDue = dateDue;
        }
    }
}
