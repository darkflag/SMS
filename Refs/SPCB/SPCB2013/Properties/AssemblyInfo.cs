using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
#if (DEBUG && CLIENTSDKV150)
[assembly: AssemblyTitle("SharePoint 2013 Client Browser")]
[assembly: AssemblyProduct("SharePoint 2013 Client Browser Preview")]
#elif (!DEBUG && CLIENTSDKV150)
[assembly: AssemblyTitle("SharePoint 2013 Client Browser")]
[assembly: AssemblyProduct("SharePoint 2013 Client Browser")]
#elif (DEBUG && CLIENTSDKV160)
[assembly: AssemblyTitle("SharePoint 2016 Client Browser")]
[assembly: AssemblyProduct("SharePoint 2016 Client Browser Preview")]
#elif (!DEBUG && CLIENTSDKV160)
[assembly: AssemblyTitle("SharePoint 2016 Client Browser")]
[assembly: AssemblyProduct("SharePoint 2016 Client Browser")]
#elif (DEBUG && CLIENTSDKV161)
[assembly: AssemblyTitle("SharePoint Online Client Browser")]
[assembly: AssemblyProduct("SharePoint Online Client Browser Preview")]
#elif (!DEBUG && CLIENTSDKV161)
[assembly: AssemblyTitle("SharePoint Online Client Browser")]
[assembly: AssemblyProduct("SharePoint Online Client Browser")]
#endif

[assembly: AssemblyDescription("The SharePoint Client Browser uses the Client Side Object Model (CSOM) to connect to a remote SharePoint Site Collection or Office 365 Tenant and shows the structure with related properties and values.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Bram de Jager (bramdejager.wordpress.com)")]
[assembly: AssemblyCopyright("Copyright © 2013-2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("5da0481b-184c-46e5-8946-ded7d35817b8")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
#if CLIENTSDKV150
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.12.0.0")]
#elif CLIENTSDKV160
[assembly: AssemblyVersion("2.0.0.0")]
[assembly: AssemblyFileVersion("2.5.0.0")]
#elif CLIENTSDKV161
[assembly: AssemblyVersion("3.0.0.0")]
[assembly: AssemblyFileVersion("3.2.0.0")]
#endif