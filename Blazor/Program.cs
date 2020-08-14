using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AzureStorage.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            //builder.Configuration.AddUserSecrets("secrets1");

            builder.Configuration.AddInMemoryCollection(new Dictionary<string,string>
            {
                ["Queue:ConnectionString"] = @"DefaultEndpointsProtocol=https;AccountName=turboqueue;AccountKey=hOowligNdujHZ76+Eu/On5QcNWHQloF9KgvsRoBWUhCMjaMlpV14ju0eMcxSbOxAHBMuTpxGnYMVTfHpi9Xrgw==;EndpointSuffix=core.windows.net"
            });

            await builder.Build().RunAsync();
        }
    }
}
