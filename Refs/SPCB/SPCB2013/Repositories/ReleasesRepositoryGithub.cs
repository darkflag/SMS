using SPBrowser.Entities;
using SPBrowser.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SPBrowser.Repositories
{
    /// <summary>
    /// Represents an implementation of <see cref="ReleasesRepository"/> for GitHub releases feed.
    /// </summary>
    /// <seealso cref="SPBrowser.Repositories.ReleasesRepository" />
    public class ReleasesRepositoryGitHub : ReleasesRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReleasesRepositoryGitHub"/> class.
        /// </summary>
        public ReleasesRepositoryGitHub() :
            base("https://github.com/bramdejager/spcb/releases.atom")
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

            var posts = repository.GetPosts();

            foreach (var feedItem in posts)
            {
                Release release = new Release() { Version = new Version() };

                Match result = versionPattern.Match(feedItem.Id);
                if (result.Success)
                {
                    Version releaseVersion = new Version(result.Value.Replace('v', ' '));
                    if (releaseVersion > release.Version)
                    {
                        release.Version = releaseVersion;
                        release.Title = feedItem.Title.Text.Trim();
                        release.Description = feedItem.Content.StripHtml().Trim();
                        release.DownloadUrl = new Uri($"https://github.com{feedItem.Links[0].Uri}");
                        release.ReleaseDate = feedItem.LastUpdatedTime.Date;
                        release.Product = (ProductType)releaseVersion.Major;

                        this.Releases.Add(release);
                    }
                }
            }
        }
    }
}
