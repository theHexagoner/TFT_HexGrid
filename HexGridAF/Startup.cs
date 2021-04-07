using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using HexGridInterfaces.Factories;
using SvgLib.Factories;
using HexGridLib.Factories;

[assembly: FunctionsStartup(typeof(HexGridAF.Startup))]

namespace HexGridAF
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IHexGridPageVmBuilder, HexGridPageVmBuilder>();
            builder.Services.AddSingleton<IGridFactory, GridFactory>();
            builder.Services.AddSingleton<ISvgGridBuilder, SvgGridBuilder>();
            builder.Services.AddSingleton<ISvgMapBuilder, SvgMapBuilder>();
            builder.Services.AddSingleton<IHitTesterFactory, HitTesterFactory>();

        }
    }
}
