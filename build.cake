#tool "Squirrel.Windows" 
#addin Cake.Squirrel

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


Task("Pack")
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