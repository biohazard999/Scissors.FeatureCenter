#tool "Squirrel.Windows" 
#addin Cake.Squirrel
#tool "nuget:?package=xunit.runner.console"
#tool "nuget:?package=GitVersion.CommandLine"

var target = string.IsNullOrEmpty(Argument("target", "Default")) ? "Default" : Argument("target", "Default");
var version = Argument("packageversion", "1.0.0.0");
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
            UpdateAssemblyInfo = false,
        });

        XmlPoke("./Scissors.FeatureCenter.Package/Package.appxmanifest", "/Package:Package/Package:Identity/@Version", gitVersion.AssemblySemVer, new XmlPokeSettings 
        {
            Namespaces = new Dictionary<string, string> {{ "Package", "http://schemas.microsoft.com/appx/manifest/foundation/windows10" }}
        });

        Information($"##vso[task.setvariable variable=AssemblySemVer]{gitVersion.AssemblySemVer}");
    });

Task("Rebuild")
    .IsDependentOn("Clean")
    .IsDependentOn("Build");

Task("Build")
    .IsDependentOn("Restore")
    .IsDependentOn("UpdateVersion")
    .Does(() => Build(configure: settings => settings
                                                .WithProperty("AppxBundle", "Never")
                                                .WithProperty("UapAppxPackageBuildMode", "CI")));
Task("Pack")
    .IsDependentOn("Restore")
    .IsDependentOn("UpdateVersion")
    .Does(() => Build(configure: settings => settings
                                                .WithProperty("AppxBundle", "Always")
                                                .WithProperty("UapAppxPackageBuildMode", "StoreUpload")));

Task("Rerelease")
    .IsDependentOn("Clean")
    .IsDependentOn("Release");

Task("Release")
    .IsDependentOn("Restore")
    .IsDependentOn("UpdateVersion")
    .Does(() => Build("Release", configure: settings => settings
                                                            .WithProperty("AppxBundle", "Always")
                                                            .WithProperty("UapAppxPackageBuildMode", "StoreUpload")));

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

// Task("Pack")
//     .IsDependentOn("Test")
//     .IsDependentOn("UITest")
//     .Does(() =>
//     {
//         NuGetPack("./Scissors.FeatureCenter.Win/Scissors.FeatureCenter.Win.nuspec", new NuGetPackSettings
//         {
//             Version = version,
//             OutputDirectory = "./bin",
//             BasePath = "./Scissors.FeatureCenter.Win/bin/Release",
//         });

//         var settings = new SquirrelSettings();
//         settings.NoMsi = true;
//         settings.Silent = true;

//         Squirrel(File($"./bin/Scissors.FeatureCenter.Win.{version}.nupkg"), settings);
//     });

Task("Default")
    .IsDependentOn("Build");

RunTarget(target);