using SPBrowser.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPBrowser.Repositories
{
    /// <summary>
    /// Base class for <see cref="Release"/> repository.
    /// </summary>
    public abstract class ReleasesRepository
    {
        /// <summary>
        /// Gets or sets the list of releases.
        /// </summary>
        /// <value>
        /// The releases.
        /// </value>
        public List<Release> Releases { get; set; }

        /// <summary>
        /// Gets the feed URL.
        /// </summary>
        /// <value>
        /// The feed URL.
        /// </value>
        public string FeedUrl { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReleasesRepository"/> class.
        /// </summary>
        /// <param name="feedUrl">The feed URL.</param>
        public ReleasesRepository(string feedUrl)
        {
            this.Releases = new List<Release>();
            this.FeedUrl = feedUrl; 
        }
    }
}
