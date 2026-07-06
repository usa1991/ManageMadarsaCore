using Microsoft.AspNetCore.Mvc;

namespace ManageMadarsaCore.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
    public IActionResult About() => View();
    public IActionResult Vision() => View();
    public IActionResult Teachers() => View();
    public IActionResult Courses() => View();
    public IActionResult Timetable() => View();
    public IActionResult Admissions() => View();
    public IActionResult Fees() => View();
    public IActionResult News() => View();
    public IActionResult Gallery() => View();
    public IActionResult Events() => View();
    public IActionResult Faq() => View();
    public IActionResult Contact() => View();
    public IActionResult Downloads() => View();
    public IActionResult Library() => View();
    public IActionResult Prayer() => View();
    public IActionResult Knowledge() => View();
}
