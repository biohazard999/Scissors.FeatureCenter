
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
}