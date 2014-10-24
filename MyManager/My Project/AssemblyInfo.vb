Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices

' General Information about an assembly is controlled through the following 
' set of attributes. Change these attribute values to modify the information
' associated with an assembly.

' Review the values of the assembly attributes

<Assembly: AssemblyTitle("MyManager")> 
<Assembly: AssemblyDescription("")> 
<Assembly: AssemblyCompany("Roberto Rutigliano")> 
<Assembly: AssemblyProduct("MyManager")> 
<Assembly: AssemblyCopyright("Copyright © - 2007")> 
<Assembly: AssemblyTrademark("")> 

<Assembly: ComVisible(False)>

'The following GUID is for the ID of the typelib if this project is exposed to COM
<Assembly: Guid("815dfeb5-2706-4f75-93e6-e5c2adfdaea7")> 

' Version information for an assembly consists of the following four values:
'
'      Major Version
'      Minor Version 
'      Build Number
'      Revision
'
' You can specify all the values or you can default the Build and Revision Numbers 
' by using the '*' as shown below:
' <Assembly: AssemblyVersion("1.0.*")> 

<Assembly: AssemblyVersion("4.0.2.1")> 
<Assembly: AssemblyFileVersion("4.0.2.1")> 
'09/01/2011 ho aggiunto questa riga perchè Aruba ha modificato il livello di sicurezza sui suoi server
<Assembly: Security.AllowPartiallyTrustedCallers()> 
