using SPBrowser.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SPBrowser.Repositories
{
    /// <summary>
    /// Represents an implementation of <see cref="ReleasesRepository"/> for Codeplex releases feed.
    /// </summary>
    /// <seealso cref="SPBrowser.Repositories.ReleasesRepository" />
    public class ReleasesRepositoryCodeplex : ReleasesRepository
    {
        private const string RELEASE_PREFIX = "Released:";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReleasesRepositoryCodeplex"/> class.
        /// </summary>
        public ReleasesRepositoryCodeplex() :
            base("https://spcb.codeplex.com/project/feeds/rss?ProjectRSSFeed=codeplex%3a%2f%2frelease%2fspcb")
        {
        }

        /// <summary>
        /// Gets the releases.
        /// </summary>
        public void GetReleases()
        {
            List<Release> releases = new List<Release>();

            Regex versionPattern = new Regex(@"v([0-9]|\.)+");

            FeedRepository repository = new FeedRepository(this.FeedUrl);

            var posts = repository.GetPosts().Where(f => f.Title.Text.StartsWith(RELEASE_PREFIX));

            foreach (var feedItem in posts)
            {
                Release release = new Release() { Version = new Version() };

                Match result = versionPattern.Match(feedItem.Title.Text);
                if (result.Success)
                {
                    Version releaseVersion = new Version(result.Value.Replace('v', ' '));
                    if (releaseVersion > release.Version)
                    {
                        release.Version = releaseVersion;
                        release.Title = feedItem.Title.Text.Substring(feedItem.Title.Text.IndexOf(':') + 1).Trim();
                        release.Description = string.Empty;
                        release.DownloadUrl = feedItem.Links[0].Uri;
                        release.ReleaseDate = feedItem.PublishDate.Date;
                        release.Product = (ProductType)releaseVersion.Major;

                        releases.Add(release);
                    }
                }
            }

            // Get distinct releases
            foreach (var group in releases.GroupBy(r => r.Title))
            {
                Release rel = group.OrderBy(r => r.ReleaseDate).FirstOrDefault();
                this.Releases.Add(rel);
            }
        }
    }
}
