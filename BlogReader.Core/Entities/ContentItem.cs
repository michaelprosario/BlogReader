using System;
using System.Collections.Generic;

namespace BlogReader.Core.Entities
{
    public class ContentItemAuthor
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class ContentItem
    {
        public string Title { get; set; } = "";
        public string Link { get; set; } = "";
        public List<ContentItemAuthor> Authors { get; set; } = new List<ContentItemAuthor>();
        public DateTime? CreationDate { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
        public string SummaryText { get; set; } = "";
        public string? Content { get; set; }
        public string ImageLink { get; set; } = "";
    }
}