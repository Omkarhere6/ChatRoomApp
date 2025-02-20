using System.Diagnostics;
using ChatRoomApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoomApp.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
