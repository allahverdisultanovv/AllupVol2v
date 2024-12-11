using Microsoft.AspNetCore.Mvc;

namespace AllupVol2.Controllers
{
    public class HomeController : Controller
    {


        public IActionResult Index()
        {
            return View();
        }

    }
}
