string nugetFeed = EnvironmentVariable("NUGET_FEED") ?? null;

public class Bld
{
	public string RootFolder = "./";
	public string SrcFolder = "./src";
	public string DemosFolder = "./demos";
	public string ArtifactsFolder = "./artifacts";
	public string ArtifactsNugetFolder => $"{ArtifactsFolder}/nuget";
	public string ArtifactsDemosFolder => $"{ArtifactsFolder}/demos";
	public string ArtifactsPackages => $"{ArtifactsFolder}/packages";
	public string ArtifactsTestResultsFolder => $"{ArtifactsFolder}/test-results";

	public string[] CleanFilters => new []
	{
		$"{SrcFolder}/**/bin/",
		$"{SrcFolder}/**/obj/",
		$"{DemosFolder}/**/bin/",
		$"{DemosFolder}/**/obj/",
	};

	public string SrcSln => "./Scissors.FeatureCenter.src.sln";
	public string DemosSln => "./Scissors.FeatureCenter.demos.sln";

	public IList<string> NugetDefaultSources => new List<string>
	{
		"https://api.nuget.org/v3/index.json"
	};

	public string ConfigurationDebug = "Debug";
	public string ConfigurationRelease = "Release";
	public string[] Configurations => new []
	{
		ConfigurationDebug,
		ConfigurationRelease
	};

	public string SrcTestFilter => $"{SrcFolder}/**/bin/**/*.*Tests*.dll";

	public string SrcAssemblyVersion { get; set; }
	public string SrcAssemblyFileVersion { get; set; }
	public string SrcInformationalVersion { get; set; }
	public string SrcNugetVersion { get; set; }
	public string DxVersion { get; set; }

	public string DemosPackageSource => $"{DemosFolder}/win10/Scissors.FeatureCenter.Package/AppPackages";
}

var bld = new Bld();
if(!string.IsNullOrEmpty(nugetFeed))
{
	bld.NugetDefaultSources.Add(nugetFeed);
}
bld.NugetDefaultSources.Add(bld.ArtifactsNugetFolder);