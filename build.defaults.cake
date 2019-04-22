string nugetFeed = EnvironmentVariable("NUGET_FEED") ?? null;

public class Bld
{
	public string RootFolder = "./";
	public string SrcFolder = "./src";
	public string DemosFolder = "./src";
	public string ArtifactsFolder = "./artifacts";
	public string ArtifactsNugetFolder => $"{ArtifactsFolder}/nuget";
	public string ArtifactsDemosFolder => $"{ArtifactsFolder}/demos";

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
}

var bld = new Bld();
if(!string.IsNullOrEmpty(nugetFeed))
{
	bld.NugetDefaultSources.Add(nugetFeed);
}
bld.NugetDefaultSources.Add(bld.ArtifactsNugetFolder);