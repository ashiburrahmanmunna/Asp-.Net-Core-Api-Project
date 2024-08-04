using GtrTraingHr.Models;
using GtrTraingHr.Models.Const;
using GtrTraingHr.Services.IService;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GtrTraingHr.ConsumeAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICompanyService companyService;
        private readonly HttpClient httpClient;

        public HomeController(ILogger<HomeController> logger,
            ICompanyService companyService,
            HttpClient httpClient )
        {
            _logger = logger;
            this.companyService = companyService;
            this.httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var companylist = await companyService.GetAll();
            return View(companylist);
        }

        public async Task<IActionResult> SetCompany(string Compid)
        {
            var com = await companyService.GetById(Compid);
            Response.Cookies.Append(ApplicationConst.CompanyHeaderName, Compid);
            Response.Cookies.Append("CompanyName", com.ComName);
            //httpClient.DefaultRequestHeaders.Add(CompanyHeaderConst.CompanyHeaderName, Compid);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}