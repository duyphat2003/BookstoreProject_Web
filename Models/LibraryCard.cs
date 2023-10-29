namespace BookstoreProject.Models
{
    public class LibraryCard
    {
        string id;
        string name;
        string expirationDate;
        bool useStatus;
        bool borrowStatus;

        public LibraryCard(string id, string name, string expirationDate, bool useStatus, bool borrowStatus)
        {
            this.id = id;
            this.name = name;
            this.expirationDate = expirationDate;
            this.useStatus = useStatus;
            this.borrowStatus = borrowStatus;
        }

        public LibraryCard(string id, string name, bool useStatus, bool borrowStatus)
        {
            this.id = id;
            this.name = name;
            this.useStatus = useStatus;
            this.borrowStatus = borrowStatus;
        }

        public LibraryCard() { }

        public string getId()
        {
            return id;
        }

        public void setId(string id)
        {
            this.id = id;
        }

        public string getName()
        {
            return name;
        }

        public void setName(string name)
        {
            this.name = name;
        }

        public string getExpirationDate()
        {
            return expirationDate;
        }

        public void setExpirationDate(string expirationDate)
        {
            this.expirationDate = expirationDate;
        }

        public bool getUseStatus()
        {
            return useStatus;
        }

        public void setUseStatus(bool useStatus)
        {
            this.useStatus = useStatus;
        }

        public bool getBorrowStatus()
        {
            return borrowStatus;
        }

        public void setBorrowStatus(bool borrowStatus)
        {
            this.borrowStatus = borrowStatus;
        }
    }
}
