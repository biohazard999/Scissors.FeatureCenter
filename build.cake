#tool "nuget:?package=xunit.runner.console"
#tool "nuget:?package=GitVersion.CommandLine"
#addin nuget:?package=Cake.Json
#addin nuget:?package=Newtonsoft.Json&version=9.0.1

#l "build.helpers.cake"

var target = string.IsNullOrEmpty(Argument("target", "Default")) ? "Default" : Argument("target", "Default");
string nugetFeed = EnvironmentVariable("NUGET_FEED") ?? null;

Information("Nuget Feed: {0}", nugetFeed);

var version = Argument("packageversion", "0.0.0.0");
GitVersion gitVersion = null;

public class Bld
{
	public string Sln { get; set; } = "./Scissors.FeatureCenter.sln";
	public string Configuration { get; set; } = "Debug";
	public string Version { get; set; }
	public string AssemblySemVer { get; set; }
	public Verbosity Verbosity { get; set; } = Verbosity.Normal;
	public int MaxCpuCount { get; set; } = 8;
	public PlatformTarget PlatformTarget { get; set; } = PlatformTarget.MSIL;

	public string OutputDirectory { get; set; } = "build";
	public string TestOutputDirectory { get; set; } = "build/testresults";
}

var bld = new Bld
{
	Version = version
};

Task("Restore")
    .Does(() => NuGetRestore(bld.Sln, new NuGetRestoreSettings
	{
        Source = nugetFeed == null ? new string[]{} : new []
		{
			"https://api.nuget.org/v3/index.json",
			nugetFeed
		}
	}));

Task("Clean")
    .WithCriteria(TFBuild.IsRunningOnAzurePipelinesHosted)
    .IsDependentOn("Clean:Force");

Task("Clean:Force")
    .Does(() =>
	{
		CleanDirectory(bld.OutputDirectory);
		Clean(bld.Sln, bld.Configuration);
	});

Task("Clean:F")
    .IsDependentOn("Clean:Force");

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

Task("Build:Debug")
    .IsDependentOn("Restore")
    .IsDependentOn("UpdateVersion")
    .Does(() => Build(bld.Sln, "Debug", configure: settings => settings
												.WithProperty("Version",  gitVersion.AssemblySemVer)
                                                .WithProperty("AppxBundle", "Never")
												.WithProperty("AppxBundlePlatforms", "Neutral")
                                                .WithProperty("UapAppxPackageBuildMode", "CI")));

Task("Build:D")
	.IsDependentOn("Build:Debug");

Task("Build:Release")
    .IsDependentOn("Restore")
    .IsDependentOn("UpdateVersion")
    .Does(() => Build(bld.Sln, "Release", configure: settings => settings
															.WithProperty("Version",  gitVersion.AssemblySemVer)
															.WithProperty("AssemblyVersion", gitVersion.AssemblySemVer)
                                                            .WithProperty("AppxBundle", "Always")
                                                            .WithProperty("UapAppxPackageBuildMode", "StoreUpload")));
Task("Build:R")
	.IsDependentOn("Build:Release");

Task("Build")
	.IsDependentOn("Build:Debug")
	.IsDependentOn("Build:Release");

Task("Test:Unit")
    .IsDependentOn("Clean")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var testAssemblies = GetFiles("./src/**/bin/**/*.*Tests*.dll");

        XUnit2(testAssemblies, new XUnit2Settings
        {
            ReportName = "TestResults_Unit",
            Parallelism = ParallelismOption.Collections,
            HtmlReport = true,
            XmlReport = true,
            OutputDirectory = bld.TestOutputDirectory,
        }
        .ExcludeTrait("Category", "UITest")
        .ExcludeTrait("Category", "Integration"));
    });

Task("Test:U")
	.IsDependentOn("Test:Unit");

Task("Test:Integration")
    .IsDependentOn("Clean")
    .IsDependentOn("Test:Unit")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var testAssemblies = GetFiles("./src/**/bin/**/*.*Tests*.dll");

        XUnit2(testAssemblies, new XUnit2Settings
        {
            ReportName = "TestResults_Integration",
            Parallelism = ParallelismOption.Collections,
            HtmlReport = true,
            XmlReport = true,
            OutputDirectory = bld.TestOutputDirectory,
        }
        .IncludeTrait("Category", "Integration"));
    });

Task("Test:I")
	.IsDependentOn("Test:Integration");

Task("Test:UI")
    .WithCriteria(!TFBuild.IsRunningOnAzurePipelinesHosted)
    .IsDependentOn("Clean")
    .IsDependentOn("Test:Unit")
    .IsDependentOn("Test:Integration")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var testAssemblies = GetFiles("./src/**/bin/Release/**/*.*Tests*.dll");

        XUnit2(testAssemblies, new XUnit2Settings
        {
            ReportName = "TestResults_UITests",
            Parallelism = ParallelismOption.Collections,
            HtmlReport = true,
            XmlReport = true,
            OutputDirectory = bld.TestOutputDirectory,
        }.IncludeTrait("Category", "UITest"));
    });

Task("Test")
    .IsDependentOn("Test:Unit")
    .IsDependentOn("Test:Integration")
    .IsDependentOn("Test:UI");

// Task("Pack")
//     .IsDependentOn("Pack.NuGet")
//     .IsDependentOn("Pack.Store")
//     ;

Task("Pack.Store")
    .IsDependentOn("Restore")
    .IsDependentOn("UpdateVersion")
    .IsDependentOn("Test")
    .Does(() => Build(bld.Sln, "Release", configure: settings => settings
                                                .WithProperty("AppxBundle", "Always")
                                                .WithProperty("UapAppxPackageBuildMode", "StoreUpload")));

// Task("Pack.NuGet")
//     .IsDependentOn("Restore")
//     .IsDependentOn("UpdateVersion")
//     .IsDependentOn("Release")
//     .Does(() =>
//     {
//         Information(SerializeJsonPretty(gitVersion));

//         NuGetPack("./Scissors.FeatureCenter.Win/Scissors.FeatureCenter.Win.nuspec", new NuGetPackSettings
//         {
//             Version = gitVersion.NuGetVersion,
//             OutputDirectory = "./bin",
//             BasePath = "./Scissors.FeatureCenter.Win/bin/Release",
//         });

//         var settings = new SquirrelSettings();
//         settings.NoMsi = true;
//         settings.Silent = true;

//         Squirrel(File($"./bin/Scissors.FeatureCenter.Win.{gitVersion.NuGetVersion}.nupkg"), settings);
//     });

Task("Default")
    .IsDependentOn("Build");

RunTarget(target);