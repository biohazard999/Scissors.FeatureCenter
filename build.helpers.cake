
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