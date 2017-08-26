using SPBrowser.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;

namespace SPBrowser.Repositories
{
    /// <summary>
    /// Represents a feed of posts which supports RSS, ATOM
    /// </summary>
    public class FeedRepository
    {
        private string _feedUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedRepository"/> class.
        /// </summary>
        /// <param name="feedUrl">The feed URL.</param>
        public FeedRepository(string feedUrl)
        {
            _feedUrl = feedUrl;
        }

        /// <summary>
        /// Gets the posts.
        /// </summary>
        /// <returns>Returns list of <see cref="SyndicationItem"/> objects.</returns>
        public List<SyndicationItem> GetPosts()
        {
            List<SyndicationItem> posts = new List<SyndicationItem>();

            if (NetworkUtil.IsConnectedToInternet())
            {
                var reader = XmlReader.Create(_feedUrl);
                var feed = SyndicationFeed.Load(reader);

                posts = feed.Items.ToList();
            }
            else
            {
                LogUtil.LogMessage("No internet connection: skipped checking for new releases.");
            }

            return posts;
        }
    }
}
