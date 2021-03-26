using HexGridInterfaces.Factories;
using HexGridInterfaces.Grids;
using HexGridInterfaces.Structs;
using HexGridInterfaces.SvgHelpers;
using SvgLib.ViewModels;

namespace SvgLib.Factories
{
    public class SvgMapBuilder : ISvgMapBuilder
    {

        public ISvgMap Build(IMap map, SvgViewBox viewBox)
        {
            return new SvgMap(map, viewBox);
        }

    }
}
