using HexGridInterfaces.Factories;
using HexGridLib.Factories;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SvgLib.Factories;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HexBlazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton<IHexGridPageVmBuilder, HexGridPageVmBuilder>();
            builder.Services.AddSingleton<ISvgGridBuilder, SvgGridBuilder>();
            builder.Services.AddSingleton<IGridFactory, GridFactory>();

            await builder.Build().RunAsync();
        }
    }
}
