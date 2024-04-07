using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using YoutubeClient.Models;

namespace YoutubeClient.Controllers
{
    public class YoutubeController : Controller
    {

        private readonly ILogger<YoutubeController> _logger;

        public YoutubeController(ILogger<YoutubeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Player()
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
