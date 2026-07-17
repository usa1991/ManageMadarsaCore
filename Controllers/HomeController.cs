using Microsoft.AspNetCore.Mvc;

namespace ManageMadarsaCore.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
    public IActionResult About() => View();
    public IActionResult Vision() => View();
    public IActionResult Contact() => View();
    public IActionResult Features()
        {
            return View();
        }

        public IActionResult Privacy() => View();
}
