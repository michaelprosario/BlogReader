using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BlogReader.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlogReader.Web.Models;

namespace BlogReader.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogReaderService _blogReaderService;

        public HomeController(ILogger<HomeController> logger, IBlogReaderService blogReaderService)
        {
            _logger = logger;
            _blogReaderService = blogReaderService;
        }

        public IActionResult Index()
        {
            var url = "http://inspiredtoeducate.net/inspiredtoeducate/feed";
            var response = _blogReaderService.GetRssFeedContent(new GetRssFeedContentCommand{ Url = url });
            var getContentItemImagesResponse = _blogReaderService.GetContentItemImages(new GetContentItemImagesCommand { ContentItems = response.ContentItems});
            response.ContentItems = getContentItemImagesResponse.ContentItems;
            return View(response);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
