#tool "nuget:?package=xunit.runner.console&version=2.4.1"

void DoBuild(string project, string[] configurations, Action<MSBuildSettings> configure = null)
{
	foreach(var configuration in configurations)
	{
		MSBuild(project, settings =>
		{
			settings.MaxCpuCount = 8;
			settings.Verbosity = Verbosity.Normal;
			settings.Configuration = configuration;
			settings.PlatformTarget = PlatformTarget.MSIL;
			settings.Restore = false;

			configure?.Invoke(settings);
		});
	}
}

void DoClean(string project, string[] configurations, Action<MSBuildSettings> configure = null)
{
	DoBuild(project, configurations, (settings) =>
	{
		settings.WithTarget("Clean");
		configure?.Invoke(settings);
	});
}

void DoTest(string testPattern, string reportType, string outputDirectory, Action<XUnit2Settings> configure = null)
{
	var settings = new XUnit2Settings
	{
		ReportName = $"TestResults_{reportType}",
		Parallelism = ParallelismOption.Collections,
		HtmlReport = true,
		XmlReport = true,
		OutputDirectory = outputDirectory
	};

	configure?.Invoke(settings);

	XUnit2(testPattern, settings);
}