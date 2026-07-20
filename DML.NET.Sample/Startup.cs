using DML.NET.Sample.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;
using ToolBX.DML.NET;

namespace DML.NET.Sample;

public class Startup : ConsoleStartup
{
    //Serialize enums (ex: ProfanityLevel) by their name rather than their numeric value, which is what a consumer
    //typically wants to see and work with.
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public Startup(IConfiguration configuration) : base(configuration)
    {
    }

    public override void Run(IServiceProvider serviceProvider)
    {
        var dmlSerializer = serviceProvider.GetRequiredService<IDmlSerializer>();

        while(true)
        {
            Console.WriteLine(Messages.EnterText);
            try
            {
                var line = Console.ReadLine();
                var dml = dmlSerializer.Deserialize(line);
                Console.WriteLine(JsonSerializer.Serialize(dml, JsonOptions));
            }
            catch (Exception exception)
            {
                Console.WriteLine(Messages.Error, exception.Message);
            }
        }
    }
}