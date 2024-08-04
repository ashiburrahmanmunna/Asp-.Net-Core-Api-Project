using GtrTraingHr.Data.Repository.Interface;
using GtrTraingHr.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GtrTraingHr.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICompanyRepo companyRepo;

        public HomeController(ILogger<HomeController> logger,ICompanyRepo companyRepo)
        {
            _logger = logger;
            this.companyRepo = companyRepo;
           
        }

        public async Task<IActionResult> Index()
        {
            var companylist = await companyRepo.GetAllAsync();
            return View(companylist);
        }

        public async Task<IActionResult> SetCompany(string Compid)
        {
            var com = await companyRepo.Single(x => x.Id == Compid);
            Response.Cookies.Append("CompanyID", Compid);
            Response.Cookies.Append("CompanyName", com.ComName);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}