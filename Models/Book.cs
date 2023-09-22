namespace BookstoreProject.Models
{
    public class Book
    {
        string id;
        string title;
        string author;
        string genre;
        string content;
        string yearPublished;
        string publisher;
        string urlImage;

        public Book(string id, string title, string author, string genre, string content, string yearPublished, string publisher, string urlImage)
        {
            this.id = id;
            this.title = title;
            this.author = author;
            this.genre = genre;
            this.content = content;
            this.yearPublished = yearPublished;
            this.publisher = publisher;
            this.urlImage = urlImage;
        }

        public Book()
        {

        }

        public string getUrlImage()
        {
            return urlImage;
        }

        public void setUrlImage(string urlImage)
        {
            this.urlImage = urlImage;
        }

        public string getId()
        {
            return id;
        }

        public void setId(string id)
        {
            this.id = id;
        }

        public string getTitle()
        {
            return title;
        }

        public void setTitle(string title)
        {
            this.title = title;
        }

        public string getAuthor()
        {
            return author;
        }

        public void setAuthor(string author)
        {
            this.author = author;
        }

        public string getGenre()
        {
            return genre;
        }

        public void setGenre(string genre)
        {
            this.genre = genre;
        }

        public string getContent()
        {
            return content;
        }

        public void setContent(string content)
        {
            this.content = content;
        }

        public string getYearPublished()
        {
            return yearPublished;
        }

        public void setYearPublished(string yearPublished)
        {
            this.yearPublished = yearPublished;
        }

        public string getPublisher()
        {
            return publisher;
        }

        public void setPublisher(string publisher)
        {
            this.publisher = publisher;
        }



    }
}
