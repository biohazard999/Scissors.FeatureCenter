#tool "Squirrel.Windows" 
#addin Cake.Squirrel
#tool "nuget:?package=xunit.runner.console"
#tool "nuget:?package=GitVersion.CommandLine"

var target = string.IsNullOrEmpty(Argument("target", "Default")) ? "Default" : Argument("target", "Default");
var version = Argument("packageversion", "1.0.0");
var sln = "./Scissors.FeatureCenter.sln";

void Build(string configuration = "Debug")
{
    var gitVersion = GitVersion(new GitVersionSettings
    {
        UpdateAssemblyInfo = true,
    });

    MSBuild(sln, settings =>
    {
        settings.MaxCpuCount = 8;
        settings.Verbosity = Verbosity.Minimal;
        settings.Configuration = configuration;
        settings.PlatformTarget = PlatformTarget.MSIL;
        settings
            .WithProperty("AssemblyVersion", new []{ gitVersion.AssemblySemVer })
            .WithProperty("Identity.Version", new []{ gitVersion.AssemblySemVer });
    });
}

void Clean(string configuration = "Debug")
{
    MSBuild(sln, settings =>
    {
        settings.MaxCpuCount = 8;
        settings.Verbosity = Verbosity.Minimal;
        settings.Configuration = "Debug";
        settings.PlatformTarget = PlatformTarget.MSIL;
        settings.WithTarget("Clean");
    });
}

Task("Restore")
    .Does(() => NuGetRestore("./Scissors.FeatureCenter.sln"));

Task("Clean")
    .Does(() =>
    {
        Clean();
        Clean("Release");
    });

Task("Rebuild")
    .IsDependentOn("Clean")
    .IsDependentOn("Build");

Task("Build")
    .IsDependentOn("Restore")
    .Does(() => Build());

Task("Rerelease")
    .IsDependentOn("Clean")
    .IsDependentOn("Release");

Task("Release")
    .IsDependentOn("Restore")
    .Does(() => Build("Release"));

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var testAssemblies = GetFiles("./src/**/bin/**/*.*Tests*.dll");

        XUnit2(testAssemblies, new XUnit2Settings 
        {
            ReportName = "TestResults",
            Parallelism = ParallelismOption.Collections,
            HtmlReport = true,
            XmlReport = true,
            OutputDirectory = "./build",
        }.ExcludeTrait("Category", "UITest"));
    });

Task("UITest")
    .IsDependentOn("Test")
    .IsDependentOn("Release")
    .Does(() =>
    {
        var testAssemblies = GetFiles("./src/**/bin/Release/**/*.*Tests*.dll");

        XUnit2(testAssemblies, new XUnit2Settings 
        {
            ReportName = "TestResults_UITests",
            Parallelism = ParallelismOption.Collections,
            HtmlReport = true,
            XmlReport = true,
            OutputDirectory = "./build",
        }.IncludeTrait("Category", "UITest"));
    });

Task("Pack")
    .IsDependentOn("Test")
    .IsDependentOn("UITest")
    .Does(() =>
    {
        NuGetPack("./Scissors.FeatureCenter.Win/Scissors.FeatureCenter.Win.nuspec", new NuGetPackSettings
        {
            Version = version,
            OutputDirectory = "./bin",
            BasePath = "./Scissors.FeatureCenter.Win/bin/Release",
        });

        var settings = new SquirrelSettings();
        settings.NoMsi = true;
        settings.Silent = true;

        Squirrel(File($"./bin/Scissors.FeatureCenter.Win.{version}.nupkg"), settings);
    });

Task("Default")
    .IsDependentOn("Build");

RunTarget(target);