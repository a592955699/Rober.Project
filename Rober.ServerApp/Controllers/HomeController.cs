using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rober.Core.Domain.Account;
using Rober.IDAL;
using Rober.ServerApp.Models;

namespace Rober.ServerApp.Controllers
{
    public class HomeController : Controller
    {
        //private IDbContext _dbContext;
        //public HomeController(IDbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        public IActionResult Index()
        {
            //var u = _dbContext.Set<User>().Find(1);
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
