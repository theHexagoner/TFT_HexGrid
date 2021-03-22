using System;
using HexGridLib.Coordinates;
using HexGridLib.Grids;

namespace DebugConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            var origin = new GridPoint(0.5d, .5d);
            var schema = new OffsetSchema(false, false, false);
            var pxRadius = (1 / Math.Sqrt(3)) * 96;
            var size = new GridPoint(pxRadius);
            var grid = new Grid(138, 106, size, origin, schema);
            _ = SvgLib.Grids.SvgGridBuilder.Build(grid);

            Console.WriteLine("Done, press any key...");
            Console.ReadKey();

        }
    }
}
