using SPBrowser.Entities;
using SPBrowser.Repositories;
using SPBrowser.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPBrowser.Managers
{
    /// <summary>
    /// Represents manager class for <see cref="Release"/>.
    /// </summary>
    /// <seealso cref="Release"/>
    public class ReleasesManager
    {
        /// <summary>
        /// Gets the latest release.
        /// </summary>
        /// <returns></returns>
        public static Release GetLatestRelease()
        {
            DateTime start = DateTime.Now;

            Release release = new Release() { Version = new Version() };

            List<Release> releases = new List<Release>();

            // TODO: remove Codeplex releases feed, after new releases are available on GitHub
            ReleasesRepositoryCodeplex codeplex = new ReleasesRepositoryCodeplex();
            codeplex.GetReleases();
            releases.AddRange(codeplex.Releases);

            ReleasesRepositoryGitHub github = new ReleasesRepositoryGitHub();
            github.GetReleases();
            releases.AddRange(github.Releases);

            // Get latest release
            release = releases.Where(r => r.Product == Globals.Product).OrderByDescending(r => r.Version).FirstOrDefault();

            LogUtil.LogMessage($"Loaded {releases.Count} releases in {(DateTime.Now - start).TotalSeconds} seconds.", LogLevel.Verbose, 0, LogCategory.Releases);

            return release;
        }
    }
}
