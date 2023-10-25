using Amazon.SimpleNotificationService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

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
