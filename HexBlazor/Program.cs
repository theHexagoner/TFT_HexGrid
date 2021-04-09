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

            var baseAddress = builder.Configuration["BaseAddress"] ?? builder.HostEnvironment.BaseAddress;
            builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(baseAddress) });
            
            builder.Services.AddSingleton<IHexGridPageVmBuilder, HexGridPageVmBuilder>();
            builder.Services.AddSingleton<IGridFactory, GridFactory>();
            //builder.Services.AddSingleton<ISvgGridBuilder, SvgGridBuilder>();
            //builder.Services.AddSingleton<ISvgMapBuilder, SvgMapBuilder>();
            //builder.Services.AddSingleton<IHitTesterFactory, HitTesterFactory>();

            await builder.Build().RunAsync();
        }
    }
}
