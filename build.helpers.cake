#tool "nuget:?package=xunit.runner.console&version=2.4.1"

void DoBuild(string project, string[] configurations, Action<MSBuildSettings> configure = null)
{
	foreach(var configuration in configurations)
	{
		MSBuild(project, settings =>
		{
			settings.MaxCpuCount = 8;
			settings.Verbosity = Verbosity.Minimal;
			settings.Configuration = configuration;
			settings.PlatformTarget = PlatformTarget.MSIL;
			settings.Restore = true;

			configure?.Invoke(settings);
		});
	}
}

void DoClean(string project, string[] configurations, Action<MSBuildSettings> configure = null)
{
	DoBuild(project, configurations, (settings) =>
	{
		settings.WithTarget("Clean");
		settings.Restore = false;
		configure?.Invoke(settings);
	});
}

void DoPack(string slnFile, string configuration, Action<MSBuildSettings> configure = null)
{
	// foreach(var project in GetProjects(slnFile))
		DoBuild(slnFile, new [] { configuration }, (settings) =>
		{
			settings.WithTarget("Pack");
			settings.Restore = false;
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