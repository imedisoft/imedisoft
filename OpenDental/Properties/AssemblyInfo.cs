using System.Reflection;
using System.Runtime.InteropServices;

// Db Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Imedisoft")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Imedisoft")]
[assembly: AssemblyCopyright("Copyright ©  2020")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("7ac7c168-694c-434b-b8cc-c351dd7fe115")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//

//The following line was added because OD was looking mangled when scaling was set above 150%. This line might make not make WPF scale as well,
//so we might consider removing it when a significant portion of OD is in WPF.
[assembly: System.Windows.Media.DisableDpiAwareness]
[assembly: AssemblyVersion("20.2.23.0")]
