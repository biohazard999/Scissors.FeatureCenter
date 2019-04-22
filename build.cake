#tool "nuget:?package=xunit.runner.console&version=2.4.1"
#tool "nuget:?package=GitVersion.CommandLine&version=4.0.0"

#l "./build.defaults.cake"

var target = string.IsNullOrEmpty(Argument("target", "Default")) ? "Default" : Argument("target", "Default");
string nugetFeed = EnvironmentVariable("NUGET_FEED") ?? null;

var bld = new Bld();

Task("Clean")
	.Description("Cleans all build artifacts")
	.Does(() =>
	{
		foreach(var filter in bld.CleanFilters)
			CleanDirectories(GetDirectories(filter));

		if(DirectoryExists(bld.ArtifactsFolder))
			DeleteDirectory(bld.ArtifactsFolder, new DeleteDirectorySettings
			{
				Force = true,
				Recursive = true
			});
	});

Task("Restore")
	.IsDependentOn("Clean")
	.Does(() =>
	{
		NuGetRestore(bld.SrcSln, new NuGetRestoreSettings
		{

		});
	});

Task("Build")
	.IsDependentOn("Restore");

Task("Pack")
	.IsDependentOn("Build");

Task("Default")
	.IsDependentOn("Pack");

RunTarget("Default");