#tool "nuget:?package=xunit.runner.console&version=2.4.1"
#tool "nuget:?package=GitVersion.CommandLine&version=4.0.0"

var target = string.IsNullOrEmpty(Argument("target", "Default")) ? "Default" : Argument("target", "Default");
string nugetFeed = EnvironmentVariable("NUGET_FEED") ?? null;

class Bld
{
	public string SrcFolder = "./src";
	public string DemosFolder = "./src";
	public string ArtifactsFolder = "./artifacts";
	public string[] CleanFilters => new []
	{
		$"{SrcFolder}/**/bin/",
		$"{SrcFolder}/**/obj/",
		$"{DemosFolder}/**/bin/",
		$"{DemosFolder}/**/obj/",
	};
}

var bld = new Bld();

Task("Clean")
	.Description("Cleans all build artifacts")
	.Does(() =>
	{
		foreach(var filter in bld.CleanFilters)
			CleanDirectories(GetDirectories(filter));

		DeleteDirectory(bld.ArtifactsFolder, new DeleteDirectorySettings
		{
			Force = true,
			Recursive = true
		});
	});

Task("Restore")
	.IsDependentOn("Clean");

Task("Build")
	.IsDependentOn("Restore");

Task("Pack")
	.IsDependentOn("Build");

Task("Default")
	.IsDependentOn("Pack");

RunTarget("Default");