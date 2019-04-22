#tool "nuget:?package=xunit.runner.console&version=2.4.1"
#tool "nuget:?package=GitVersion.CommandLine&version=4.0.0"

#l "./build.defaults.cake"
#l "./build.helpers.cake"

var target = string.IsNullOrEmpty(Argument("target", "Default")) ? "Default" : Argument("target", "Default");

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

		DoClean(bld.SrcSln, bld.Configurations);
	});

Task("Restore")
	.IsDependentOn("Clean")
	.Does(() =>
	{
		NuGetRestore(bld.SrcSln, new NuGetRestoreSettings
		{
			Source = bld.NugetDefaultSources,
			NoCache = true,
		});
	});

Task("Build:src")
	.IsDependentOn("Restore")
	.Does(() => DoBuild(bld.SrcSln, bld.Configurations));

Task("Pack:src")
	.IsDependentOn("Build:src");

Task("Default")
	.IsDependentOn("Pack:src");

RunTarget("Default");