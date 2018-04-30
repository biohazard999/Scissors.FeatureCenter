#tool "Squirrel.Windows"
#addin Cake.Squirrel
#tool "nuget:?package=xunit.runner.console"
#tool "nuget:?package=GitVersion.CommandLine"
#addin nuget:?package=Cake.Json
#addin nuget:?package=Newtonsoft.Json&version=9.0.1

var target = string.IsNullOrEmpty(Argument("target", "Default")) ? "Default" : Argument("target", "Default");
var version = Argument("packageversion", "0.0.0.0");
var sln = "./Scissors.FeatureCenter.sln";
GitVersion gitVersion = null;


void Build(string configuration = "Debug", Action<MSBuildSettings> configure = null)
{
    MSBuild(sln, settings =>
    {
        settings.MaxCpuCount = 8;
        settings.Verbosity = Verbosity.Normal;
        settings.Configuration = configuration;
        settings.PlatformTarget = PlatformTarget.MSIL;

        settings
            .WithProperty("AssemblyVersion", gitVersion.AssemblySemVer)
            .WithProperty("AppxBundlePlatforms", "Neutral")
            ;

        configure?.Invoke(settings);
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

Task("UpdateVersion")
    .Does(() =>
    {
        gitVersion = GitVersion(new GitVersionSettings
        {
            UpdateAssemblyInfo = true,
        });

        version = gitVersion.MajorMinorPatch + "." + gitVersion.CommitsSinceVersionSource;

        XmlPoke("./Scissors.FeatureCenter.Package/Package.appxmanifest", "/Package:Package/Package:Identity/@Version", version, new XmlPokeSettings
        {
            Namespaces = new Dictionary<string, string> {{ "Package", "http://schemas.microsoft.com/appx/manifest/foundation/windows10" }}
        });

        Information($"##vso[task.setvariable variable=AssemblySemVer]{version}");
    });

Task("Rebuild")
    .IsDependentOn("Clean")
    .IsDependentOn("Build");

Task("Build")
    .IsDependentOn("Restore")
    .IsDependentOn("UpdateVersion")
    .Does(() => Build(configure: settings => settings
												.WithProperty("Version",  gitVersion.AssemblySemVer)
                                                .WithProperty("AppxBundle", "Never")
                                                .WithProperty("UapAppxPackageBuildMode", "CI")));
Task("Rerelease")
    .IsDependentOn("Clean")
    .IsDependentOn("Release");

Task("Release")
    .IsDependentOn("Restore")
    .IsDependentOn("UpdateVersion")
    .Does(() => Build("Release", configure: settings => settings
															.WithProperty("Version",  gitVersion.AssemblySemVer)
                                                            .WithProperty("AppxBundle", "Always")
                                                            .WithProperty("UapAppxPackageBuildMode", "StoreUpload")));
Task("Test")
    .IsDependentOn("Test.Unit")
    .IsDependentOn("Test.Integration")
    .IsDependentOn("Test.UI");

Task("Test.Unit")
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
        }
        .ExcludeTrait("Category", "UITest")
        .ExcludeTrait("Category", "Integration"));
    });

Task("Test.Integration")
    .IsDependentOn("Test.Unit")
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
        }
        .IncludeTrait("Category", "Integration"));
    });

Task("Test.UI")
    .WithCriteria(!TFBuild.IsRunningOnVSTS)
    .IsDependentOn("Test.Unit")
    .IsDependentOn("Test.Integration")
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
    .IsDependentOn("Pack.NuGet")
    .IsDependentOn("Pack.Store")
    ;

Task("Pack.Store")
    .IsDependentOn("Restore")
    .IsDependentOn("UpdateVersion")
    .IsDependentOn("Test")
    .Does(() => Build(configure: settings => settings
                                                .WithProperty("AppxBundle", "Always")
                                                .WithProperty("UapAppxPackageBuildMode", "StoreUpload")));

Task("Pack.NuGet")
    .IsDependentOn("Restore")
    .IsDependentOn("UpdateVersion")
    .IsDependentOn("Release")
    .Does(() =>
    {
        Information(SerializeJsonPretty(gitVersion));

        NuGetPack("./Scissors.FeatureCenter.Win/Scissors.FeatureCenter.Win.nuspec", new NuGetPackSettings
        {
            Version = gitVersion.NuGetVersion,
            OutputDirectory = "./bin",
            BasePath = "./Scissors.FeatureCenter.Win/bin/Release",
        });

        var settings = new SquirrelSettings();
        settings.NoMsi = true;
        settings.Silent = true;

        Squirrel(File($"./bin/Scissors.FeatureCenter.Win.{gitVersion.NuGetVersion}.nupkg"), settings);
    });

Task("Default")
    .IsDependentOn("Build");

RunTarget(target);