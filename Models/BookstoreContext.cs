using Microsoft.EntityFrameworkCore;

namespace BookstoreProject.Models
{
    public class BookstoreContext:DbContext
    {
        public BookstoreContext(DbContextOptions<BookstoreContext> options)
           : base(options)
        {
        }
    }
   
}
