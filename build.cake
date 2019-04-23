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
		DoClean(bld.DemosSln, bld.Configurations);
		if(!DirectoryExists(bld.ArtifactsNugetFolder))
			CreateDirectory(bld.ArtifactsNugetFolder);
	});

Task("Version:src")
	.Does(() =>
	{
		var version = GitVersion(new GitVersionSettings
		{
			UpdateAssemblyInfo = false,
		});
		bld.SrcAssemblyVersion = version.AssemblySemVer;
		bld.SrcAssemblyFileVersion = version.AssemblySemFileVer;
		bld.SrcInformationalVersion = version.InformationalVersion;
		bld.SrcNugetVersion = version.NuGetVersionV2;
		bld.DxVersion = $"{version.Major}.{version.Minor}.7";

		Information($"SrcAssemblyVersion: {bld.SrcAssemblyVersion}");
		Information($"SrcAssemblyFileVersion: {bld.SrcAssemblyFileVersion}");
		Information($"SrcInformationalVersion: {bld.SrcInformationalVersion}");
		Information($"SrcNugetVersion: {bld.SrcNugetVersion}");
		Information($"DxVersion: {bld.DxVersion}");
	});

Task("Build:src")
	.IsDependentOn("Clean")
	.IsDependentOn("Version:src")
	.Does(() => DoBuild(bld.SrcSln, bld.Configurations, settings =>
		settings
			.WithProperty("DxVersion", bld.DxVersion)
			.WithProperty("Version", bld.SrcAssemblyVersion)
			.WithProperty("FileVersion", bld.SrcAssemblyFileVersion)
			.WithProperty("InformationalVersion", bld.SrcInformationalVersion)));

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
	.IsDependentOn("Test:src")
	.Does(() => DoPack(bld.SrcSln, bld.ConfigurationRelease, (settings) => settings
		.WithProperty("NoBuild", "True")
		.WithProperty("PackageVersion", bld.SrcNugetVersion)
		.WithProperty("PackageVersionPrefix", "")
		.WithProperty("PackageVersionSuffix", "")
		));

Task("Build:demos")
	.IsDependentOn("Pack:src")
	.Does(() => DoBuild(bld.DemosSln, bld.Configurations, settings =>
		settings
			.WithProperty("DxVersion", bld.DxVersion)
			.WithProperty("ScissorsVersion", bld.SrcNugetVersion)
			.WithProperty("Version", bld.SrcAssemblyVersion)
			.WithProperty("FileVersion", bld.SrcAssemblyFileVersion)
			.WithProperty("InformationalVersion", bld.SrcInformationalVersion)));

Task("Copy:demos")
	.IsDependentOn("Build:demos")
	.Does(() =>
	{
		if(!DirectoryExists(bld.ArtifactsPackages))
			CreateDirectory(bld.ArtifactsPackages);

		CopyDirectory(bld.DemosPackageSource, bld.ArtifactsPackages);
	});

Task("Default")
	.IsDependentOn("Copy:demos");

RunTarget(target);