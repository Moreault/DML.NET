using ToolBX.AutoInject.Generated;

namespace ToolBX.DML.NET;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds support for the Dialog Markup Language.
    /// </summary>
    public static IServiceCollection AddDml(this IServiceCollection services, AutoInjectOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        options ??= new AutoInjectOptions();
        services.AddAwesomeMarkup(options.DefaultLifetime);
        //Ensures IOptions<DmlOptions> always resolves - to its defaults when neither AddAutoConfig nor the
        //DmlOptions overload below configured it.
        services.AddOptions();
        AutoInjectRegistrar.Register(services, options.DefaultLifetime);
        return services;
    }

    /// <summary>
    /// Adds support for the Dialog Markup Language with explicit <see cref="DmlOptions"/>, bypassing appsettings.
    /// Use this when you'd rather configure DML in code than bind it from configuration via
    /// <c>services.AddAutoConfig(configuration)</c>.
    /// </summary>
    public static IServiceCollection AddDml(this IServiceCollection services, DmlOptions dmlOptions, AutoInjectOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(dmlOptions);
        services.AddDml(options);
        //Registered last so this explicit instance wins over the default registered by the overload above.
        services.AddSingleton<IOptions<DmlOptions>>(Options.Create(dmlOptions));
        return services;
    }
}