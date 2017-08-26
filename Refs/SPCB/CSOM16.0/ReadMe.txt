https://dev.office.com/blogs/sharepoint-csom-versions-for-on-premises-released-as-nuget-packages

We are happy to announce availability of SharePoint Client Side Object Model (CSOM) assemblies as NuGet packages for SharePoint 2013 and SharePoint 2016. Previously we have shipped CSOM only as seperate installer packages (msi's), but wanted to ensure that you can use exactly the same assemblies also using NuGet packages. You can find these packages from the NuGet gallery with specific id's for SharePoint 2013 and 2016 versions.

SharePoint 2013 CSOM NuGet package - Microsoft.SharePoint2013.CSOM
SharePoint 2016 CSOm NuGet package -  Microsoft.SharePoint2016.CSOM

These packages contain exactly the same versions of the assemblies as the msi packages to avoid version conflicts. We are also committed to update these NuGet packages in the same time as any updates for the msi packages. This means that it does not really matter which option you want to use in our deployments. Key advantage of having these assemblies as NuGet packages is to have possibility to reference them as NuGet in your Visual Studio solutions without any external dependencies. Versions of the CSOM assemblies are as follows, which also aligns with the previously released msi packages.

SharePoint 2013 CSOM - April 2015 CU
SharePoint 2016 CSOM - SP2016 RTM version