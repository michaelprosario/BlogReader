using System;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Xml.Linq;
using Ardalis.GuardClauses;
using BlogReader.Core.Entities;
using BlogReader.Core.Interfaces;
using BlogReader.Core.Services;
using HtmlAgilityPack;

namespace BlogReader.Infrastructure
{
    public class ConcreteRssService : IRssService
    {
        public GetRssFeedContentResponse GetRssFeedContent(GetRssFeedContentCommand command)
        {
            Guard.Against.Null(command, nameof(command));
            using var reader = XmlReader.Create(command.Url);
            var feed = SyndicationFeed.Load(reader);

            var response = SetupResponse(feed);
            foreach (var feedItem in feed.Items)
            {
                var contentItem = MakeContentItem(feedItem);
                response.ContentItems.Add(contentItem);
            }

            foreach (var contentItem in response.ContentItems)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(contentItem.SummaryText);
                var imageNodes = doc.DocumentNode.SelectNodes("//img");
                foreach (var imageNode in imageNodes) imageNode.ParentNode.RemoveChild(imageNode);
                contentItem.SummaryTextWithNoImages = doc.DocumentNode.OuterHtml;
            }

            response.Link = command.Url;

            return response;
        }

        public string GetImageLinkFromRssContent(ContentItem contentItem)
        {
            Guard.Against.Null(contentItem, nameof(contentItem));

            string content;
            if (!string.IsNullOrEmpty(contentItem.Content))
                content = contentItem.Content;
            else if (!string.IsNullOrEmpty(contentItem.SummaryText))
                content = contentItem.SummaryText;
            else
                return "";

            var doc = new HtmlDocument();
            doc.LoadHtml(content);
            var firstImageNode = doc.DocumentNode.SelectSingleNode("//img");
            return firstImageNode != null ? firstImageNode.Attributes["src"].Value : "";
        }

        private ContentItem MakeContentItem(SyndicationItem feedItem)
        {
            var contentItem = new ContentItem();
            foreach (var author in contentItem.Authors)
            {
                var contentItemAuthor = new ContentItemAuthor {Email = author.Email, Name = author.Name};
                contentItem.Authors.Add(contentItemAuthor);
            }

            contentItem.SummaryText = feedItem.Summary.Text;
            foreach (var category in feedItem.Categories) contentItem.Categories.Add(category.Name);

            contentItem.Link = feedItem.Links.First().Uri.AbsoluteUri;
            contentItem.Title = feedItem.Title.Text;
            contentItem.CreationDate = feedItem.PublishDate.DateTime;
            contentItem.Content = feedItem.ToString();

            var contentItemsFound = 0;
            foreach (var ext in feedItem.ElementExtensions)
                if (ext.GetObject<XElement>().Name.LocalName == "encoded")
                {
                    contentItem.Content = ext.GetObject<XElement>().Value;
                    contentItemsFound++;
                }

            if (contentItemsFound == 0) contentItem.Content = "";

            if (contentItemsFound > 1) Console.WriteLine("Interesting: found more than one content on feed item");

            return contentItem;
        }

        private GetRssFeedContentResponse SetupResponse(SyndicationFeed feed)
        {
            return new GetRssFeedContentResponse
            {
                Description = feed.Description.Text,
                Title = feed.Title.Text,
                LastBuildDate = feed.LastUpdatedTime.Date
            };
        }
    }
}