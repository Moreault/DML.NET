using ToolBX.AutoInject.Generated;

namespace ToolBX.DML.NET;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds support for the Dialog Markup Language. Do not call if you're using AutoInject in your project.
    /// </summary>
    public static IServiceCollection AddDml(this IServiceCollection services, AutoInjectOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        options ??= new AutoInjectOptions();
        services.AddAwesomeMarkup(options.DefaultLifetime);
        AutoInjectRegistrar.Register(services, options.DefaultLifetime);
        return services;
    }
}