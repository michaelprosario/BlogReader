using BlogReader.Core.Interfaces;
using BlogReader.Core.Services;
using BlogReader.Infrastructure;
using NUnit.Framework;

namespace BlogReader.Integration.Tests
{
    public class Tests
    {
        private IRssService _rssService;
        private BlogReaderService _service;

        [SetUp]
        public void Setup()
        {
            _rssService = new ConcreteRssService();
            _service = new BlogReaderService(_rssService);
        }

        [Test]
        public void Service__GetImagesFromContent__ItShouldWork()
        {
            // arrange
            // act
            var getRssFeedContentResponse = _service.GetRssFeedContent(new GetRssFeedContentCommand
            {
                Url = "https://makezine.com/feed/"
            });

            // act
            var response = _service.GetContentItemImages(new GetContentItemImagesCommand
            {
                ContentItems = getRssFeedContentResponse.ContentItems
            });

            // assert
            Assert.IsTrue(response.ContentItems.Count > 0);
            foreach (var contentItem in response.ContentItems)
                Assert.IsTrue(!string.IsNullOrEmpty(contentItem.ImageLink));
        }

        [Test]
        public void Service__RssFeedContent__MichaelHyatt()
        {
            // arrange
            var command = new GetRssFeedContentCommand
            {
                Url = "https://michaelhyatt.com/feed/"
            };

            // act
            var response = _service.GetRssFeedContent(command);

            // assert 
            Assert.NotNull(response);
        }

        [Test]
        public void Service__RssFeedContent__MichaelHyattNoImagesInSummaryText()
        {
            // arrange
            var command = new GetRssFeedContentCommand
            {
                Url = "https://michaelhyatt.com/feed/"
            };

            // act
            var response = _service.GetRssFeedContent(command);

            // assert 
            foreach (var item in response.ContentItems)
                Assert.IsTrue(item.SummaryTextWithNoImages.IndexOf("<img") == -1);
        }


        [Test]
        public void Service__RssFeedContent__MyBlog()
        {
            // arrange
            var command = new GetRssFeedContentCommand
            {
                Url = "http://inspiredtoeducate.net/inspiredtoeducate/feed/"
            };

            // act
            var response = _service.GetRssFeedContent(command);

            // assert 
            Assert.NotNull(response);
            foreach (var item in response.ContentItems) Assert.True(!string.IsNullOrEmpty(item.Content));
        }
    }
}