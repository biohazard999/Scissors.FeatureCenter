string nugetFeed = EnvironmentVariable("NUGET_FEED") ?? null;
string storeDir = EnvironmentVariable("STORE_DIR") ?? null;

Information("Nuget Feed: {0}", nugetFeed);
Information("Store Dir: {0}", storeDir);

public class Bld
{
	public string RootFolder = "./";
	public string ScriptsFolder = "./scripts";
	public string SrcFolder = "./src";
	public string DemosFolder = "./demos";
	public string ArtifactsFolder = "./artifacts";
	public string ArtifactsNugetFolder => $"{ArtifactsFolder}/nuget";
	public string ArtifactsDemosFolder => $"{ArtifactsFolder}/demos";
	public string ArtifactsNugetFolderAbsolute { get; set; }
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

	public IList<string> NugetDefaultSources { get; } = new List<string>
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
	public string SrcDocs { get; set;} = "./docs/docfx.json";
	public string DxVersion { get; set; } = "2";
	public string DxVersionDemos { get; set; } = "2";
	public string DemosVersion { get; set; }

	public string StoreFilesDir { get; set; }
	public string DemosPackageFolder => $"{DemosFolder}/win10/Scissors.FeatureCenter.Package";
	public string DemosPackageSource => $"{DemosPackageFolder}/AppPackages";
	public string NugetSources => String.Join(";", NugetDefaultSources) + ";";
}

var bld = new Bld();
if(!string.IsNullOrEmpty(nugetFeed))
{
	bld.NugetDefaultSources.Add(nugetFeed);
}
if(!string.IsNullOrEmpty(storeDir))
{
	bld.StoreFilesDir = storeDir;
}
bld.ArtifactsNugetFolderAbsolute = Directory(bld.ArtifactsNugetFolder).Path.MakeAbsolute(Context.Environment).ToString().Replace("/", @"\");