namespace ToolBX.DML.NET;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds support for the Dialog Markup Language. Do not call if you're using AutoInject in your project.
    /// </summary>
    public static IServiceCollection AddDml(this IServiceCollection services, AutoInjectOptions? options = null)
    {
        return services.AddAutoInjectServices(Assembly.GetExecutingAssembly(), options);
    }
}