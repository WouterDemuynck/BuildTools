BuildTools README
-----------------

You have just installed the BuildTools NuGet package. We have prepared your solution for
supporting the BuildTools automatic versioning MSBuild tasks and registered it with the
target project in your solution. We have also added an initial major and minor version
property to your project.

When you want to customize the version number, you should unload your project and edit
the raw MSBuild file, just find and edit the <MajorVersion> and <MinorVersion> properties:

<PropertyGroup>
	<MajorVersion>2</MajorVersion>
	<MinorVersion>0</MinorVersion>
</PropertyGroup>

If you want to customize the AssemblyInformationalVersionAttribute in the generated assembly 
information file, you should add a ServicePackNumber and/or HotfixNumber property as well:

<PropertyGroup>
	<MajorVersion>2</MajorVersion>
	<MinorVersion>0</MinorVersion>
	<ServicePackNumber>1</ServicePackNumber>
</PropertyGroup>

Also, remember to remove the AssemblyVersionAttribute, AssemblyFileVersionAttribute and
AssemblyInformationalVersionAttribute entries from your existing AssemblyInfo.cs file, or
you will not be able to build your project (because only one instance of each of those
attributes is allowed in an assembly).