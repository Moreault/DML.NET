using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToolBX.AssemblyInitializer;
using ToolBX.AutoConfig;
using ToolBX.DML.NET;

namespace DML.NET.Sample;

/// <summary>
/// Wires up the sample : registers DML (which also registers the AwesomeMarkup parser it depends on) and binds the
/// "Dml" section of appsettings.json onto <see cref="DmlOptions"/> so the sample can demonstrate configuration-driven
/// behaviour (ex: the profanity clean fallback.) <see cref="DmlOptions"/> carries the <c>[AutoConfig("Dml")]</c>
/// attribute, so AutoConfig discovers and binds it.
/// </summary>
public sealed class DmlSampleInitializer : IAssemblyInitializer
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDml();
        services.AddAutoConfig(typeof(DmlOptions).Assembly, configuration);
    }

    public void Configure(IInitializerContext context)
    {
    }
}
