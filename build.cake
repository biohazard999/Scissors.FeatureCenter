#tool "nuget:?package=GitVersion.CommandLine&version=4.0.0"

#l "./build.defaults.cake"
#l "./build.helpers.cake"

var target = string.IsNullOrEmpty(Argument("target", "Default")) ? "Default" : Argument("target", "Default");
GitVersion version = null;

Task("Version:Git")
	.Does(() =>
	{
		var gitVersionResult = GitVersion(new GitVersionSettings
		{
			UpdateAssemblyInfo = false,
		});
		version = gitVersionResult;
	});

Task("Version")
	.IsDependentOn("Version:Git");

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
	.IsDependentOn("Version");

Task("Build:src")
	.IsDependentOn("Restore")
	.Does(() => DoBuild(bld.SrcSln, bld.Configurations, settings =>
	{
		settings.Restore = true;
		settings.WithProperty("DxVersion", $"{version.Major}.{version.Minor}.*");
	}));

Task("Test:src:Unit")
	.IsDependentOn("Build:src")
	.Does(() => DoTest(bld.SrcTestFilter, "Unit", bld.ArtifactsTestResultsFolder, (settings) => settings
		.ExcludeTrait("Category", "UITest")
        .ExcludeTrait("Category", "Integration")));

Task("Test:src:Integration")
	.IsDependentOn("Build:src")
	.Does(() => DoTest(bld.SrcTestFilter, "Integration", bld.ArtifactsTestResultsFolder, (settings) => settings
        .IncludeTrait("Category", "Integration")));

Task("Test:src")
	.IsDependentOn("Test:src:Unit")
	.IsDependentOn("Test:src:Integration");

Task("Pack:src")
	.IsDependentOn("Test:src");

Task("Default")
	.IsDependentOn("Pack:src");

RunTarget("Default");