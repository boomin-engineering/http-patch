using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HttpPatch.Tests.Api;

public class TestApi : WebApplicationFactory<SystemTextStartup>
{
    private readonly SerializationEngine _serializationEngine;

    public TestApi(SerializationEngine serializationEngine)
    {
        _serializationEngine = serializationEngine;
    }

    public TestDataStore DataStore => Services.GetRequiredService<TestDataStore>();    
        
    protected override IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseEnvironment("Development");
                webBuilder.UseUrls("http://0.0.0.0:3000");
                
                switch(_serializationEngine)
                {
                    case SerializationEngine.Newtonsoft:
                        webBuilder.UseStartup<NewtonsoftStartup>();
                        break;
                    case SerializationEngine.SystemText:
                        webBuilder.UseStartup<SystemTextStartup>();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                webBuilder.ConfigureTestServices(sc =>
                {
                    sc.AddSingleton<TestDataStore>();
                });
            });        
    }
}