using BookstoreProject.Models;
using Google.Cloud.Firestore;
using System.Net;

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

        public static List<Book> books; // sách
        public static List<Genre> genres; // thể loại
        public static List<Copy> copies; // bản sao của sách
        public static List<Account> accounts;
        public static List<LibraryCard> libraryCards;
        public static List<Loan> loans;

        public static List<string> bookName; // từ khóa gợi ý khi tìm kiếm

        public static Account accountInfo;
        public static LibraryCard libraryCard;

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
            genres = new List<Genre>();
            Task<QuerySnapshot> genreIds = genreCollectionRef.GetSnapshotAsync();
            genreIds.Wait();
            foreach (DocumentSnapshot genreId in genreIds.Result)
            {
                genres.Add(new Genre(genreId.GetValue<string>("Id"),
                                     genreId.GetValue<string>("Name")));
            }
        }
        // Tải sách
        public static void LoadBooks()
        {
            books = new List<Book>();
            
            Task<QuerySnapshot> bookIds = bookCollectionRef.GetSnapshotAsync();
            bookIds.Wait();

            foreach (DocumentSnapshot bookId in bookIds.Result)
            {
                string content = "";
                foreach (string arCon in bookId.GetValue<List<string>>("Content"))
                {
                    content += arCon + "\n";
                }

                books.Add(new Book(bookId.GetValue<string>("Id"),
                                     bookId.GetValue<string>("Name"),
                                     bookId.GetValue<string>("Author"),
                                     bookId.GetValue<string>("Genre"),
                                     content,
                                     bookId.GetValue<string>("YearPublished"),
                                     bookId.GetValue<string>("Publisher"),
                                     bookId.GetValue<string>("URL")));

                Console.WriteLine(bookId.GetValue<string>("Name"));
            }
        }


        public static Book LoadContentBookWithId(string id)
        {
            Book book = new Book();
            Task<QuerySnapshot> bookIds = bookCollectionRef.WhereEqualTo("Id", id).GetSnapshotAsync();
            bookIds.Wait();
            foreach (DocumentSnapshot bookId in bookIds.Result)
            {
                string content = "";
                foreach (string arCon in bookId.GetValue<List<string>>("Content"))
                {
                    content += arCon + "\n";
                }

                book = new Book(bookId.GetValue<string>("Id"),
                                     bookId.GetValue<string>("Name"),
                                     bookId.GetValue<string>("Author"),
                                     bookId.GetValue<string>("Genre"),
                                     content,
                                     bookId.GetValue<string>("YearPublished"),
                                     bookId.GetValue<string>("Publisher"),
                                     bookId.GetValue<string>("URL"));

                Console.WriteLine(bookId.GetValue<string>("Name"));

            }
            return book;
        }

        public static List<Book> booksAfterSorted;
        // Tải sách
        public static void LoadBooksSortedWithCopies()
        {
            List<Tuple<string, int>> booksBeforeSorted = new List<Tuple<string, int>>();
            booksAfterSorted = new List<Book>();
            foreach (Book book in books)
            {
                Task<QuerySnapshot> copyIds = copyCollectionRef.Document(book.getId()).Collection("BookCopy").GetSnapshotAsync();
                copyIds.Wait();
                int i = 0;
                foreach (DocumentSnapshot id in copyIds.Result)
                {
                    if (id.GetValue<string>("Status").Equals("Cho mượn"))
                    {
                        i++;
                    }
                }
                booksBeforeSorted.Add(new Tuple<string, int>(book.getId(), i));
            }


            for(int i = 0; i < booksBeforeSorted.Count - 1; i++)
            {
                for(int j = i + 1; j < booksBeforeSorted.Count; j++)
                {
                    if (booksBeforeSorted[i].Item2 < booksBeforeSorted[j].Item2)
                    {
                        Tuple<string, int> temp = booksBeforeSorted[i];
                        booksBeforeSorted[i] = booksBeforeSorted[j];
                        booksBeforeSorted[j] = temp;
                    }
                }
            }

            foreach(Tuple<string, int> tuple in booksBeforeSorted)
            {
                foreach (Book book in books)
                {
                    if(tuple.Item1.Equals(book.getId()))
                    {
                        booksAfterSorted.Add(book);
                        break;
                    }
                }
            }
        }

        // tải các bản sao của sách
        public static void LoadCopies()
        {
            copies = new List<Copy>();

            foreach (Book book in books)
            {
                Task<QuerySnapshot> copyIds = copyCollectionRef.Document(book.getId()).Collection("BookCopy").GetSnapshotAsync();
                while (true)
                {
                    if (copyIds.IsCompleted)
                    {
                        foreach (DocumentSnapshot copy in copyIds.Result)
                        {
                            copies.Add(new Copy(copy.Id,
                                    book.getId(),
                                    copy.GetValue<string>("Status"),
                                    copy.GetValue<string>("Notes")));
                            Console.WriteLine("Copy id " + copy.Id + ", book id " + book.getId());
                        }
                        break;
                    }
                }
            }
        }

        

        public static List<Copy> LoadCopiesWithBookId(string bookId, string status)
        {
            List<Copy> copyArrayList = new List<Copy>();

            Task<QuerySnapshot> copyIds = copyCollectionRef.Document(bookId).Collection("BookCopy").GetSnapshotAsync();
            copyIds.Wait();
            foreach (DocumentSnapshot id in copyIds.Result)
            {
                if (!string.IsNullOrEmpty(status) && id.GetValue<string>("Status").Equals(status))
                {
                    copyArrayList.Add(new Copy(id.Id,
                            bookId,
                            id.GetValue<string>("Status"),
                            id.GetValue<string>("Notes")));
                }
            }
            return copyArrayList;
        }

        //Pagination
        public static void LoadBooksWithIntitalStatePage(int  page, int pageSize)
        {
            books = new List<Book>();

            Task<QuerySnapshot> bookIds = bookCollectionRef.GetSnapshotAsync();
            bookIds.Wait();
            bookIds.GetAwaiter().ToString().Skip((page - 1) * pageSize).Take(pageSize);

            foreach (DocumentSnapshot bookId in bookIds.Result)
            {
                string content = "";
                foreach (string arCon in bookId.GetValue<List<string>>("Content"))
                {
                    content += arCon + "\n";
                }

                books.Add(new Book(bookId.GetValue<string>("Id"),
                                     bookId.GetValue<string>("Name"),
                                     bookId.GetValue<string>("Author"),
                                     bookId.GetValue<string>("Genre"),
                                     content,
                                     bookId.GetValue<string>("YearPublished"),
                                     bookId.GetValue<string>("Publisher"),
                                     bookId.GetValue<string>("URL")));

                Console.WriteLine(bookId.GetValue<string>("Name"));
            }
        }


        // Tải sách với bản sao mà sách có tên chứa ký tự tìm kiếm
        public static void SearchBook(string name)
        {
            books = new List<Book>();
            Task<QuerySnapshot> bookIds = bookCollectionRef.GetSnapshotAsync();
            bookIds.Wait();
            foreach (DocumentSnapshot id in bookIds.Result)
            {
                if (id.GetValue<string>("Name").Contains(name))
                {
                    string content = "";
                    foreach (string arCon in id.GetValue<List<string>>("Content"))
                    {
                        content += arCon + "\n";
                    }
                    books.Add(new Book(id.Id,
                            id.GetValue<string>("Name"),
                            id.GetValue<string>("Author"),
                            id.GetValue<string>("Genre"),
                            content,
                            id.GetValue<string>("YearPublished"),
                            id.GetValue<string>("Publisher"),
                            id.GetValue<string>("URL")));
                    Console.WriteLine("Book name " + id.Id + " : " + id.GetValue<string>("Name"));
                }
            }

            copies = new List<Copy>();

            foreach (Book book in books)
            {
                Task<QuerySnapshot> copyIds = copyCollectionRef.Document(book.getId()).Collection("BookCopy").GetSnapshotAsync();
                copyIds.Wait();
                foreach (DocumentSnapshot copy in copyIds.Result)
                {
                    copies.Add(new Copy(copy.Id,
                            book.getId(),
                            copy.GetValue<string>("Status"),
                            copy.GetValue<string>("Notes")));
                    Console.WriteLine("Copy id " + copy.Id + ", book id " + book.getId());
                }
            }
        }

        // Tải sách theo thể loại
        public static void LoadBooksWithGenre(string genreName)
        {
            books = new List<Book>();

            string content = "";
            Task<QuerySnapshot> bookIds = bookCollectionRef.WhereEqualTo("Genre", genreName).GetSnapshotAsync();

            bookIds.Wait();
            foreach (DocumentSnapshot id in bookIds.Result)
            {
                content = "";
                foreach (string arCon in id.GetValue<List<string>>("Content"))
                {
                    content += arCon + "\n";
                }
                books.Add(new Book(id.Id,
                        id.GetValue<string>("Name"),
                        id.GetValue<string>("Author"),
                        id.GetValue<string>("Genre"),
                        content,
                        id.GetValue<string>("YearPublished"),
                        id.GetValue<string>("Publisher"),
                        id.GetValue<string>("URL")));
                Console.WriteLine("Book name " + id.Id + " : " + id.GetValue<string>("Name"));
            }

            copies = new List<Copy>();

            foreach (Book book in books)
            {
                Task<QuerySnapshot> copyIds = copyCollectionRef.Document(book.getId()).Collection("BookCopy").GetSnapshotAsync();
                copyIds.Wait();
                foreach (DocumentSnapshot copy in copyIds.Result)
                {
                    copies.Add(new Copy(copy.Id,
                            book.getId(),
                            copy.GetValue<string>("Status"),
                            copy.GetValue<string>("Notes")));
                    Console.WriteLine("Copy id " + copy.Id + ", book id " + book.getId());
                }
            }
        }

        // Login
        public static void SearchAccount(string account, string password)
        {
            accountInfo = new Account();
            Task<QuerySnapshot> accountName = accountCollectionRef.WhereEqualTo("Account", account).GetSnapshotAsync();
            accountName.Wait();
            if (accountName.Result.Count != 0)
            {
                foreach (DocumentSnapshot name in accountName.Result)
                    if (name.GetValue<string>("Password").Equals(password))
                        accountInfo = new Account(name.GetValue<string>("Account"), name.GetValue<string>("Password"), name.GetValue<string>("Role"));

                if (string.IsNullOrEmpty(accountInfo.getAccount()))
                    Console.WriteLine("Tài khoản hoặc mật khẩu bị sai");
            }
            else
                Console.WriteLine("Tài khoản hoặc mật khẩu bị sai");

            if (!string.IsNullOrEmpty(accountInfo.getRole()) && accountInfo.getRole().Equals("Sinh viên"))
            {
                Task<QuerySnapshot> libraryCardInfo = libraryCardCollectionRef.WhereEqualTo("Id", accountInfo.getAccount()).GetSnapshotAsync();
                libraryCardInfo.Wait();
                foreach (DocumentSnapshot idCard in libraryCardInfo.Result)
                    libraryCard = new LibraryCard(accountInfo.getAccount(),
                            idCard.GetValue<string>("Name"),
                            idCard.GetValue<string>("ExpirationDate"),
                            idCard.GetValue<bool>("Status"),
                            idCard.GetValue<bool>("Borrow"));

                Console.WriteLine("Đăng nhập thành công");
            }
        }


        // Tải Tài khoản - Manager
        public static void LoadAccounts()
        {
            accounts = new List<Account>();
            Task<QuerySnapshot> accountNames = accountCollectionRef.GetSnapshotAsync();
            accountNames.Wait();
            foreach (DocumentSnapshot accountName in accountNames.Result)
            {
                accounts.Add(new Account(accountName.GetValue<string>("Account"), accountName.GetValue<string>("Password"), accountName.GetValue<string>("Role")));
            }
        }

        // tải thẻ sinh viên - Manager
        public static void LoadLibraryCards()
        {
            libraryCards = new List<LibraryCard>();
            Task<QuerySnapshot> libraryCardIds = libraryCardCollectionRef.GetSnapshotAsync();
            libraryCardIds.Wait();
            foreach (DocumentSnapshot libraryCardId in libraryCardIds.Result)
            {
                libraryCards.Add(new LibraryCard(libraryCardId.GetValue<string>("Id"),
                        libraryCardId.GetValue<string>("Name"),
                        libraryCardId.GetValue<string>("ExpirationDate"),
                        libraryCardId.GetValue<bool>("Status"),
                        libraryCardId.GetValue<bool>("Borrow")));
            }
        }

        // tải thẻ sinh viên - Manager
        public static void LoadLibraryCardsWithId(string id)
        {
            libraryCards = new List<LibraryCard>();
            Task<QuerySnapshot> libraryCardIds = libraryCardCollectionRef.GetSnapshotAsync();
            libraryCardIds.Wait();
            foreach (DocumentSnapshot libraryCardId in libraryCardIds.Result)
            {
                if (libraryCardId.GetValue<string>("Id").Contains(id))
                    libraryCards.Add(new LibraryCard(libraryCardId.GetValue<string>("Id"),
                            libraryCardId.GetValue<string>("Name"),
                            libraryCardId.GetValue<string>("ExpirationDate"),
                            libraryCardId.GetValue<bool>("Status"),
                            libraryCardId.GetValue<bool>("Borrow")));
            }
        }

        // tải thẻ sinh viên - Manager
        public static void LoadLibraryCardsWithName(string name)
        {
            libraryCards = new List<LibraryCard>();
            Task<QuerySnapshot> libraryCardIds = libraryCardCollectionRef.GetSnapshotAsync();
            libraryCardIds.Wait();
            foreach (DocumentSnapshot libraryCardId in libraryCardIds.Result)
            {
                if (libraryCardId.GetValue<string>("Name").Contains(name))
                    libraryCards.Add(new LibraryCard(libraryCardId.GetValue<string>("Id"),
                            libraryCardId.GetValue<string>("Name"),
                            libraryCardId.GetValue<string>("ExpirationDate"),
                            libraryCardId.GetValue<bool>("Status"),
                            libraryCardId.GetValue<bool>("Borrow")));
            }
        }

        public static void SortAcccount(string roleName, bool isAsc)
        {
            accounts = new List<Account>();

            Task<QuerySnapshot> accountNames;
            if (isAsc)
                accountNames = accountCollectionRef.WhereEqualTo("Role", roleName).OrderBy("Account").GetSnapshotAsync();
            else
                accountNames = accountCollectionRef.WhereEqualTo("Role", roleName).OrderByDescending("Account").GetSnapshotAsync();

            accountNames.Wait();
            foreach (DocumentSnapshot accountName in accountNames.Result)
            {
                accounts.Add(new Account(accountName.GetValue<string>("Account"), accountName.GetValue<string>("Password"), accountName.GetValue<string>("Role")));
                Console.WriteLine(accountName.GetValue<string>("Account"));
            }
        }
        //tạo lấy danh sách role
        public static void GetAccountRoles()
        {
            List<string> roles = new List<string>();
            Task<QuerySnapshot> accountIds = accountCollectionRef.GetSnapshotAsync();
            accountIds.Wait();

            foreach (DocumentSnapshot accountId in accountIds.Result)
            {
                
                string role = accountId.GetValue<string>("Role");

                if (!string.IsNullOrEmpty(role) && !roles.Contains(role))
                {
                    roles.Add(role);
                }
            }

        }


        public static void SortBookWithName(bool isAsc)
        {
            books = new List<Book>();
            string content = "";
            Task<QuerySnapshot> bookIds;
            if (isAsc)
                bookIds = bookCollectionRef.OrderBy("Name").GetSnapshotAsync();
            else
                bookIds = bookCollectionRef.OrderByDescending("Name").GetSnapshotAsync();

            bookIds.Wait();
            foreach (DocumentSnapshot id in bookIds.Result)
            {
                content = "";
                foreach (string arCon in id.GetValue<List<string>>("Content"))
                {
                    content += arCon + "\n";
                }
                books.Add(new Book(id.Id,
                        id.GetValue<string>("Name"),
                        id.GetValue<string>("Author"),
                        id.GetValue<string>("Genre"),
                        content,
                        id.GetValue<string>("YearPublished"),
                        id.GetValue<string>("Publisher"),
                        id.GetValue<string>("URL")));
                Console.WriteLine("Book name " + id.Id + " : " + id.GetValue<string>("Name"));
            }
        }

        public static void SortBookWithYearPublished(bool isAsc)
        {
            books = new List<Book>();
            string content = "";
            Task<QuerySnapshot> bookIds;
            if (isAsc)
                bookIds = bookCollectionRef.OrderBy("YearPublished").GetSnapshotAsync();
            else
                bookIds = bookCollectionRef.OrderByDescending("YearPublished").GetSnapshotAsync();

            bookIds.Wait();
            foreach (DocumentSnapshot id in bookIds.Result)
            {
                content = "";
                foreach (string arCon in id.GetValue<List<string>>("Content"))
                {
                    content += arCon + "\n";
                }
                books.Add(new Book(id.Id,
                        id.GetValue<string>("Name"),
                        id.GetValue<string>("Author"),
                        id.GetValue<string>("Genre"),
                        content,
                        id.GetValue<string>("YearPublished"),
                        id.GetValue<string>("Publisher"),
                        id.GetValue<string>("URL")));
                Console.WriteLine("Book name " + id.Id + " : " + id.GetValue<string>("Name"));
            }
        }

        public static void SortLibraryCard(bool isAsc)
        {
            libraryCards = new List<LibraryCard>();
            Task<QuerySnapshot> libraryCardIds;
            if (isAsc)
                libraryCardIds = libraryCardCollectionRef.OrderBy("Id").GetSnapshotAsync();
            else
                libraryCardIds = libraryCardCollectionRef.OrderByDescending("Id").GetSnapshotAsync();

            libraryCardIds.Wait();
            foreach (DocumentSnapshot libraryCardId in libraryCardIds.Result)
            {
                libraryCards.Add(new LibraryCard(libraryCardId.GetValue<string>("Id"),
                        libraryCardId.GetValue<string>("Name"),
                        libraryCardId.GetValue<string>("ExpirationDate"),
                        libraryCardId.GetValue<bool>("Status"),
                        libraryCardId.GetValue<bool>("Borrow")));
            }
        }

        public static void LoadLoan()
        {
            loans = new List<Loan>();
            Task<QuerySnapshot> loanIds = loanCollectionRef.GetSnapshotAsync();
            loanIds.Wait();
            foreach (DocumentSnapshot loanId in loanIds.Result)
            {
                foreach (Book book in books)
                {
                    Task<QuerySnapshot> bookCopyIds = loanCollectionRef.Document(loanId.Id).Collection(book.getId()).GetSnapshotAsync();
                    bookCopyIds.Wait();
                    foreach (DocumentSnapshot bookCopyId in bookCopyIds.Result)
                    {
                        loans.Add(new Loan(book.getId(),
                                loanId.Id,
                                bookCopyId.GetValue<string>("BookCopyId"),
                                bookCopyId.GetValue<string>("BorrowDate"),
                                bookCopyId.GetValue<string>("DateDue")));
                        Console.WriteLine(book.getId() + ", " + loanId.Id);
                    }
                }
            }
        }

        public static void SortLoan(bool isAsc)
        {
            loans = new List<Loan>();
            Task<QuerySnapshot> loanIds = loanCollectionRef.GetSnapshotAsync();
            loanIds.Wait();
            foreach (DocumentSnapshot loanId in loanIds.Result)
            {
                foreach (Book book in books)
                {
                    Task<QuerySnapshot> bookCopyIds;
                    if (isAsc)
                        bookCopyIds = loanCollectionRef.Document(loanId.Id).Collection(book.getId()).OrderBy("BorrowDate").GetSnapshotAsync();
                    else
                        bookCopyIds = loanCollectionRef.Document(loanId.Id).Collection(book.getId()).OrderByDescending("BorrowDate").GetSnapshotAsync();

                    bookCopyIds.Wait();
                    foreach (DocumentSnapshot bookCopyId in bookCopyIds.Result)
                    {
                        loans.Add(new Loan(book.getId(),
                                loanId.Id,
                                bookCopyId.Id,
                                bookCopyId.GetValue<string>("BorrowDate"),
                                bookCopyId.GetValue<string>("DateDue")));
                    }
                }
            }
        }

        public static void SortCopies(bool isAsc)
        {
            copies = new List<Copy>();

            Task<QuerySnapshot> bookIds;
            if (isAsc)
                bookIds = copyCollectionRef.OrderBy("Id").GetSnapshotAsync();
            else
                bookIds = copyCollectionRef.OrderByDescending("Id").GetSnapshotAsync();
            
            bookIds.Wait();
            foreach (DocumentSnapshot id in bookIds.Result)
            {
                Task<QuerySnapshot> copyIds = copyCollectionRef.Document(id.Id).Collection("BookCopy").GetSnapshotAsync();
                copyIds.Wait();
                foreach (DocumentSnapshot copy in copyIds.Result)
                {
                    copies.Add(new Copy(copy.Id,
                            id.Id,
                            copy.GetValue<string>("Status"),
                            copy.GetValue<string>("Notes")));
                }
            }
        }

        // Thêm sách - Manager
        public static void AddBook(Book book)
        {
            List<string> contentParts = new List<string>();
            string[] parts = book.getContent().Split("\n");

            foreach (string part in parts)
            {
                contentParts.Add(part);
            }


            Dictionary<string, object> newBook = new Dictionary<string, object>();
            newBook.Add("Author", book.getAuthor());
            newBook.Add("Content", contentParts);
            newBook.Add("Genre", book.getGenre());
            newBook.Add("Id", book.getId());
            newBook.Add("Name", book.getTitle());
            newBook.Add("Publisher", book.getPublisher());
            newBook.Add("URL", book.getUrlImage());
            newBook.Add("YearPublished", book.getYearPublished());

            bookCollectionRef.Document(book.getId()).SetAsync(newBook);
            Console.WriteLine("Thêm sách thành công: " + newBook);
        }

        // cập nhập sách - Manager
        public static void UpdateBook(Book book)
        {
            List<string> contentParts = new List<string>();
            string[] parts = book.getContent().Split("\n");

            foreach (string part in parts)
            {
                contentParts.Add(part);
            }


            Dictionary<string, object> newBook = new Dictionary<string, object>();
            newBook.Add("Author", book.getAuthor());
            newBook.Add("Content", contentParts);
            newBook.Add("Genre", book.getGenre());
            newBook.Add("Name", book.getTitle());
            newBook.Add("Publisher", book.getPublisher());
            newBook.Add("URL", book.getUrlImage());
            newBook.Add("YearPublished", book.getYearPublished());

            bookCollectionRef.Document(book.getId()).UpdateAsync(newBook);
            Console.WriteLine("Chỉnh sửa sách thành công: " + newBook);
        }

        // xóa sách - Manager
        public static void DeleteBook(string id) => bookCollectionRef.Document(id).DeleteAsync();
        // Thêm bản sao của sách - Manager
        public static void AddBookCopy(Copy copy)
        {
            Dictionary<string, object> newBookCopy = new Dictionary<string, object>();

            newBookCopy.Add("Id", copy.getBookId());
            copyCollectionRef.Document(copy.getBookId()).SetAsync(newBookCopy);

            newBookCopy = new Dictionary<string, object>();
            newBookCopy.Add("Notes", copy.getNotes());
            newBookCopy.Add("Status", copy.getStatus());

            copyCollectionRef.Document(copy.getBookId()).Collection("BookCopy").Document(copy.getId()).SetAsync(newBookCopy);
        }
        // cập nhật bản sao của sách - Manager
        public static void UpdateBookCopy(Copy copy)
        {
            Dictionary<string, object> updateBookCopy = new Dictionary<string, object>();

            updateBookCopy.Add("Notes", copy.getNotes());
            updateBookCopy.Add("Status", copy.getStatus());

            copyCollectionRef.Document(copy.getBookId()).Collection("BookCopy").Document(copy.getId()).UpdateAsync(updateBookCopy);
        }
        // xóa bản sao của sách - Manager
        public static void DeleteBookCopy(string BookId, string id) => copyCollectionRef.Document(BookId).Collection("BookCopy").Document(id).DeleteAsync();
        // Thêm thẻ thư viện - Manager
        public static bool AddLibraryCard(LibraryCard libraryCard)
        {
            if (libraryCardCollectionRef.Document(libraryCard.getId()) != null)
            {
                return false;
            }

            Dictionary<string, object> libraryCardData = new Dictionary<string, object>();

            libraryCardData.Add("Borrow", libraryCard.getBorrowStatus());
            libraryCardData.Add("ExpirationDate", libraryCard.getExpirationDate());
            libraryCardData.Add("Id", libraryCard.getId());
            libraryCardData.Add("Name", libraryCard.getName());
            libraryCardData.Add("Status", libraryCard.getUseStatus());

            libraryCardCollectionRef.Document(libraryCard.getId()).SetAsync(libraryCardData);
            return true;
        }
        // cập nhật thẻ thư viện
        public static void UpdateLibraryCard(LibraryCard libraryCard)
        {
            libraryCardCollectionRef.Document(libraryCard.getId()).UpdateAsync("Borrow", libraryCard.getBorrowStatus());
            libraryCardCollectionRef.Document(libraryCard.getId()).UpdateAsync("Status", libraryCard.getUseStatus());
        }
        // xóa thẻ thư viện - Manager
        public static void DeleteLibraryCard(string id) => libraryCardCollectionRef.Document(id).DeleteAsync();
        // Thêm Lần mượn sách- Manager
        public static void AddLoan(Loan loan)
        {
            Dictionary<string, object> multiData = new Dictionary<string, object>();
            multiData.Add("Id", loan.getCardId());
            loanCollectionRef.Document(loan.getCardId()).SetAsync(multiData);

            multiData = new Dictionary<string, object>();
            multiData.Add("BorrowDate", loan.getDateLoaned());
            multiData.Add("DateDue", loan.getDateDue());
            multiData.Add("BookCopyId", loan.getCopyId());
            loanCollectionRef.Document(loan.getCardId()).Collection(loan.getBookId()).Document().SetAsync(multiData);
        }

        // Thêm tài khoản - Manager
        public static bool AddAccount(Account account)
        {
            if (accountCollectionRef.Document(account.getAccount()) != null)
            {
                return false;
            }
            Dictionary<string, object> newAccount = new Dictionary<string, object>();
            newAccount.Add("Account", account.getAccount());
            newAccount.Add("Password", account.getPassword());
            newAccount.Add("Role", account.getRole());
            newAccount.Add("isLogin", false);

            accountCollectionRef.Document(account.getAccount()).SetAsync(newAccount);
            return true;
        }
        // Cập nhật tài khoản - Manager
        public static bool UpdateAccount(String account, String password)
        {
            Task<QuerySnapshot> accId = accountCollectionRef.WhereEqualTo("Account", account).GetSnapshotAsync();
            while (true)
            {
                if (accId.IsCompleted)
                {
                    if (accId.Result.Count != 0)
                    {
                        accountCollectionRef.Document(account).UpdateAsync("Password", password);
                        return true;
                    }
                    else
                        return false;
                }
            }
        }

        // Cập nhật tài khoản - Manager
        public static bool UpdateAccount(string account, bool isLogin)
        {
            Task<QuerySnapshot> accId = accountCollectionRef.WhereEqualTo("Account", account).GetSnapshotAsync();
            while (true)
            {
                if (accId.IsCompleted)
                {
                    if (accId.Result.Count != 0)
                    {
                        accountCollectionRef.Document(account).UpdateAsync("isLogin", isLogin);
                        return true;
                    }
                    else
                        return false;
                }
            }
        }

        // Xóa tài khoản - Manager
        public static void DeleteAccount(string nameAccount) => accountCollectionRef.Document(nameAccount).DeleteAsync();
    }
}
