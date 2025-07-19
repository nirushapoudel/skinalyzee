using Microsoft.AspNetCore.Mvc;

namespace Skinalyze.API.Controllers
{
    public class adminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
