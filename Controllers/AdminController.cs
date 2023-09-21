using BookstoreProject.Firestore_Database;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
            //_auth = new FirebaseAuthProvider(new FirebaseConfig(_apiKey));
        }

        public IActionResult Index()
        {
            BookstoreProjectDatabase.ConnectToFirestoreDB();
            return View();
        }
    }
}
