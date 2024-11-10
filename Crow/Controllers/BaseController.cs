using Microsoft.AspNetCore.Mvc;

namespace Crow.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
