using BookstoreProject.Firestore_Database;
using BookstoreProject.Models;
using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace BookstoreProject.Controllers
{
    public class BookGenreController : Controller
    {
        private readonly ILogger<BookGenreController> _logger;
        private FirebaseAuthProvider _auth;
        private static string _apiKey = "AIzaSyCjA_B3IWRegYfuYpe_j_7sPbbksrBbbDI";
        public BookGenreController(ILogger<BookGenreController> logger)
        {
            _logger = logger;
            _auth = new FirebaseAuthProvider(new FirebaseConfig(_apiKey));
        }

        public IActionResult Index(string nameGenre)
        {
            BookstoreProjectDatabase.LoadBooksWithGenre(nameGenre);
            return View();
        }
      
    }
}
