#tool "nuget:?package=GitVersion.CommandLine&version=4.0.0"
#addin "nuget:?package=Cake.DocFx&version=0.12.0"
#tool "nuget:?package=docfx.console&version=2.42.2"

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
		bld.DxVersion = $"{version.Major}.{version.Minor}.{bld.DxVersion}";
		bld.DxVersionDemos = $"{version.Major}.{version.Minor}.{bld.DxVersionDemos}";
        bld.DemosVersion = $"{version.Major}.{version.Minor}.{version.CommitsSinceVersionSource}.0";

		Information($"SrcAssemblyVersion: {bld.SrcAssemblyVersion}");
		Information($"SrcAssemblyFileVersion: {bld.SrcAssemblyFileVersion}");
		Information($"SrcInformationalVersion: {bld.SrcInformationalVersion}");
		Information($"SrcNugetVersion: {bld.SrcNugetVersion}");
		Information($"DxVersion: {bld.DxVersion}");
		Information($"DxVersionDemos: {bld.DxVersionDemos}");
		Information($"DemosVersion: {bld.DemosVersion}");
	});

Task("Build:src")
	.IsDependentOn("Clean")
	.IsDependentOn("Version:src")
	.Does(() => DoBuild(bld.SrcSln, bld.Configurations, settings =>
		settings
			.WithProperty("RestoreSources", $"{bld.NugetSources}")
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

Task("Docs:src")
	.Does(() =>
	{
		CreateDirectory(bld.ArtifactsFolder);
		CopyFiles($"{bld.ScriptsFolder}/*.*", bld.ArtifactsFolder);
		DocFxMetadata(bld.SrcDocs);
		DocFxBuild(bld.SrcDocs);
	});

Task("Pack:src")
	.IsDependentOn("Test:src")
	.IsDependentOn("Docs:src")
	.Does(() => DoPack(bld.SrcSln, bld.ConfigurationRelease, (settings) => settings
		.WithProperty("NoBuild", "True")
		.WithProperty("PackageVersion", bld.SrcNugetVersion)
		.WithProperty("PackageOutputPath", bld.ArtifactsNugetFolderAbsolute)
		.WithProperty("PackageVersionPrefix", "")
		.WithProperty("PackageVersionSuffix", "")
		));

Task("Version:demos")
	.IsDependentOn("Version:src")
	.Does(() =>
	{
        XmlPoke("./demos/win10/Scissors.FeatureCenter.Package/Package.appxmanifest", "/Package:Package/Package:Identity/@Version", bld.DemosVersion, new XmlPokeSettings
        {
            Namespaces = new Dictionary<string, string> {{ "Package", "http://schemas.microsoft.com/appx/manifest/foundation/windows10" }}
        });
		Information($"DemosVersion: {bld.DemosVersion}");
	});
Task("Prepare:demos")
	.WithCriteria(() => !string.IsNullOrEmpty(bld.StoreFilesDir))
	.Does(() =>
	{
		CopyFiles($"{bld.StoreFilesDir}/*.*", bld.DemosPackageFolder);
	});

Task("Build:demos")
	.IsDependentOn("Pack:src")
	.IsDependentOn("Version:demos")
	.IsDependentOn("Prepare:demos")
	.Does(() => DoBuild(bld.DemosSln, bld.Configurations, settings =>
		settings
			.WithProperty("RestoreSources", $"{bld.NugetSources}{bld.ArtifactsNugetFolderAbsolute};")
			.WithProperty("DxVersion", bld.DxVersionDemos)
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