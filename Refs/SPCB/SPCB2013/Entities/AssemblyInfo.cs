using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SPBrowser.Entities
{
    /// <summary>
    /// Represents detailed information regarding <see cref="Assembly"/> objects.
    /// </summary>
    public class AssemblyInfo
    {
        /// <summary>
        /// Gets the display name of the assembly.
        /// </summary>
        public string FullName { get; private set; }

        // Gets the file name of the assembly.
        public string FileName { get; private set; }

        /// <summary>
        /// Gets the path or UNC location of the loaded file that contains the manifest.
        /// </summary>
        public string Location { get; private set; }

        /// <summary>
        /// Gets the version of the product this file is distributed with.
        /// </summary>
        public Version Version { get; private set; }

        /// <summary>
        /// Gets the file version of the assembly.
        /// </summary>
        public Version FileVersion { get; private set; }

        /// <summary>
        /// Gets the public key token of the assembly.
        /// </summary>
        public string PublicKeyToken { get; private set; }

        /// <summary>
        /// Default constructor for <see cref="AssemblyInfo"/>.
        /// </summary>
        /// <param name="assembly"></param>
        public AssemblyInfo(Assembly assembly)
        {
            this.FullName = assembly.FullName;
            this.PublicKeyToken = assembly.FullName.Split(',')[3].Split('=')[1];
            this.Version = new Version(assembly.FullName.Split(',')[1].Split('=')[1]);

            try
            {
                this.Location = assembly.Location;

                this.FileName = Path.GetFileName(assembly.Location);
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                this.FileVersion = new Version(fvi.ProductVersion);
            }
            catch (NotSupportedException)
            { }
        }

        /// <summary>
        /// Gets the assemblies that have been loaded into the execution context of this application domain.
        /// </summary>
        /// <returns></returns>
        public static List<AssemblyInfo> GetAssemblies()
        {
            List<AssemblyInfo> assemblies = new List<AssemblyInfo>();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                assemblies.Add(new AssemblyInfo(assembly));
            }

            return assemblies;
        }
    }
}
