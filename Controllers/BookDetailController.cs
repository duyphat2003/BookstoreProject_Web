using Microsoft.AspNetCore.Mvc;

namespace BookstoreProject.Controllers
{
    public class BookDetailController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
