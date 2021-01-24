using BlogReader.Core.Entities;
using BlogReader.Core.Services;

namespace BlogReader.Core.Interfaces
{
    public interface IRssService
    {
        GetRssFeedContentResponse GetRssFeedContent(GetRssFeedContentCommand command);
        string GetImageLinkFromRssContent(ContentItem contentItem);
    }
}