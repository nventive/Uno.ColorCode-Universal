#addin "Cake.Powershell"

using System;
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var baseDir = MakeAbsolute(Directory("../")).ToString();
var buildDir = baseDir + "/build";
var toolsDir = buildDir + "/tools";

var Solution = baseDir + "/ColorCode.sln";
var nupkgDir = buildDir + "/nupkg";

var gitVersioningVersion = "2.1.23";
var versionClient = toolsDir + "/nerdbank.gitversioning/tools/Get-Version.ps1";
string Version = null;

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    Information("\nCleaning Package Directory");
    CleanDirectory(nupkgDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(Solution);
});

Task("Version")
    .Description("Updates the version information in all Projects")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    Information("\nDownloading NerdBank GitVersioning...");
    var installSettings = new NuGetInstallSettings {
        ExcludeVersion  = true,
        Version = gitVersioningVersion,
        OutputDirectory = toolsDir
    };
    
    NuGetInstall(new []{"nerdbank.gitversioning"}, installSettings);

    Information("\nRetrieving version...");
    var results = StartPowershellFile(versionClient);
    Version = results[1].Properties["NuGetPackageVersion"].Value.ToString();
    Information("\nBuild Version: " + Version);
});

Task("Build")
    .IsDependentOn("Version")
    .Does(() =>
{
    Information("\nBuilding Solution");
    var buildSettings = new MSBuildSettings
    {
        MaxCpuCount = 0,
		MSBuildPlatform = MSBuildPlatform.x86
    }
    .SetConfiguration("Release")
    .WithTarget("Restore;Build")
    .WithProperty("IncludeSymbols", "true")
    .WithProperty("GenerateLibraryLayout", "true")
    .WithProperty("PackageOutputPath", nupkgDir)
    .WithProperty("GeneratePackageOnBuild", "true");

    EnsureDirectoryExists(nupkgDir);
    MSBuild(Solution, buildSettings);
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
