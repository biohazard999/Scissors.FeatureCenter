void UpdateVersionInfo(string assemblyInfoFile, Func<Version, Version> callback = null)
{
    var assemblyInfo = ParseAssemblyInfo(assemblyInfoFile);
    var assemblyVersion = Version.Parse(assemblyInfo.AssemblyVersion);

    if(callback != null) assemblyVersion = callback(assemblyVersion);
    var gitVersion = GitVersion();
    var sha = gitVersion.Sha;
    var branch = gitVersion.BranchName;
    Information($"Version: {assemblyVersion}");
    Information($"Sha: {sha}");

    CreateAssemblyInfo(assemblyInfoFile, new AssemblyInfoSettings
    {
        Configuration = assemblyInfo.Configuration,
        Company = assemblyInfo.Company,
        Description = assemblyInfo.Description,
        Product = assemblyInfo.Product,
        Copyright = assemblyInfo.Copyright,
        Trademark = assemblyInfo.Trademark,

        Version = assemblyVersion.ToString(),
        FileVersion = assemblyVersion.ToString(),
        InformationalVersion = $"{assemblyVersion}+{sha}+{branch}",
    });
}

void Build(string project, string configuration, Action<MSBuildSettings> configure = null)
{
    MSBuild(project, settings =>
    {
        settings.MaxCpuCount = 8;
        settings.Verbosity = Verbosity.Normal;
        settings.Configuration = configuration;
        settings.PlatformTarget = PlatformTarget.MSIL;

        configure?.Invoke(settings);
    });
}

void Clean(string project, string configuration = "Debug", Action<MSBuildSettings> configure = null)
{
    MSBuild(project, settings =>
    {
        settings.MaxCpuCount = 8;
        settings.Verbosity = Verbosity.Minimal;
        settings.Configuration = configuration;
        settings.PlatformTarget = PlatformTarget.MSIL;
        settings.WithTarget("Clean");
		configure?.Invoke(settings);
    });
}