using System;
using TFT_HexGrid.Coordinates;
using TFT_HexGrid.Grids;

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

            _ = new Grid(69, 53, size, origin, schema);

            Console.ReadKey();

        }
    }
}
