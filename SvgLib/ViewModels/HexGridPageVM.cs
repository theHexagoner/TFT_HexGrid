using HexGridInterfaces.SvgHelpers;
using HexGridInterfaces.ViewModels;

namespace SvgLib.ViewModels
{
    public class HexGridPageVM : IHexGridPageVM
    {
        private HexGridPageVM() { }

        internal HexGridPageVM(ISvgGrid grid, ISvgMap map)
        {
            Grid = grid;
            Map = map;
        }

        public ISvgGrid Grid { get; }

        public ISvgMap Map { get; }
    }
}