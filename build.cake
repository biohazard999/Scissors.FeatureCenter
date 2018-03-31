#tool "Squirrel.Windows" 
#addin Cake.Squirrel
#tool "nuget:?package=xunit.runner.console"

var target = Argument("target", "Default");
var version = Argument("packageversion", "1.0.0");

void Build(string configuration = "Debug")
{
    MSBuild("./Scissors.FeatureCenter.sln", settings =>
    {
        settings.MaxCpuCount = 8;
        settings.Verbosity = Verbosity.Minimal;
        settings.Configuration = configuration;
        settings.PlatformTarget = PlatformTarget.MSIL;
    });
}

Task("Build")
    .Does(() =>
    {
        Build();
    });


Task("Release")
    .Does(() =>
    {
        Build("Release");
    });

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