using Microsoft.Extensions.DependencyInjection;

namespace ToolBX.DML.NET;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds support for the Dialog Markup Language. Do not call if you're using AutoInject in your project.
    /// </summary>
    public static IServiceCollection AddDml(this IServiceCollection services)
    {
        return services
            .AddSingleton<IDmlColorTagConverter, DmlColorTagConverter>()
            .AddSingleton<IDmlTextStyleConverter, DmlTextStyleConverter>()
            .AddSingleton<IDmlConverter, DmlConverter>()
            .AddSingleton<IDmlSerializer, DmlSerializer>();
    }
}