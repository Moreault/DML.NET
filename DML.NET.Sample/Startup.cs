using DML.NET.Sample.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ToolBX.DML.NET;

namespace DML.NET.Sample;

public class Startup : ConsoleStartup
{
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
                Console.WriteLine(JsonConvert.SerializeObject(dml, Formatting.Indented));
            }
            catch (Exception exception)
            {
                Console.WriteLine(Messages.Error, exception.Message);
            }
        }
    }
}