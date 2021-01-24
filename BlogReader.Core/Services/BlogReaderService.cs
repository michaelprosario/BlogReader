using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using BlogReader.Core.Entities;
using BlogReader.Core.Interfaces;

namespace BlogReader.Core.Services
{
    public class GetRssFeedContentCommand
    {
        public string Url { get; set; } = "";
    }

    public class GetRssFeedContentResponse
    {
        public string Title { get; set; } = "";
        public string Link { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime? LastBuildDate { get; set; }
        public List<ContentItem> ContentItems { get; set; } = new List<ContentItem>();
    }

    public class GetContentItemImagesCommand
    {
        public List<ContentItem> ContentItems = new List<ContentItem>();
    }

    public class GetContentItemImagesResponse
    {
        public List<ContentItem> ContentItems = new List<ContentItem>();
    }

    public interface IBlogReaderService
    {
        GetRssFeedContentResponse GetRssFeedContent(GetRssFeedContentCommand command);
        GetContentItemImagesResponse GetContentItemImages(GetContentItemImagesCommand command);
    }

    public class BlogReaderService : IBlogReaderService
    {
        private readonly IRssService _service;

        public BlogReaderService(IRssService service)
        {
            Guard.Against.Null(service, nameof(service));
            _service = service;
        }

        public GetRssFeedContentResponse GetRssFeedContent(GetRssFeedContentCommand command)
        {
            Guard.Against.Null(command, nameof(command));
            return _service.GetRssFeedContent(command);
        }

        public GetContentItemImagesResponse GetContentItemImages(GetContentItemImagesCommand command)
        {
            Guard.Against.Null(command, nameof(command));
            var response = new GetContentItemImagesResponse();
            foreach (var contentItem in command.ContentItems)
            {
                var imageLink = _service.GetImageLinkFromRssContent(contentItem);
                contentItem.ImageLink = !string.IsNullOrEmpty(imageLink) ? imageLink : "";
                response.ContentItems.Add(contentItem);
            }

            return response;
        }
    }
}