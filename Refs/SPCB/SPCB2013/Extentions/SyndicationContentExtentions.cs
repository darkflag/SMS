using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;

namespace SPBrowser.Extentions
{
    public static class SyndicationContentExtentions
    {
        public static string StripHtml(this SyndicationContent content)
        {
            string text = string.Empty;

            if (content != null && content is TextSyndicationContent)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(((TextSyndicationContent)content).Text);

                text = doc.DocumentNode.InnerText;
            }

            return text;
        }
    }
}
