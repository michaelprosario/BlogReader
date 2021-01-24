using System.Diagnostics;
using BlogReader.Core.Services;
using BlogReader.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlogReader.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBlogReaderService _blogReaderService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IBlogReaderService blogReaderService)
        {
            _logger = logger;
            _blogReaderService = blogReaderService;
        }

        public IActionResult Index()
        {
            var url = "https://makezine.com/feed/";
            var response = _blogReaderService.GetRssFeedContent(new GetRssFeedContentCommand {Url = url});
            var getContentItemImagesResponse = _blogReaderService.GetContentItemImages(new GetContentItemImagesCommand
                {ContentItems = response.ContentItems});
            response.ContentItems = getContentItemImagesResponse.ContentItems;
            return View(response);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}