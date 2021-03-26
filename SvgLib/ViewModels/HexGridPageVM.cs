using HexGridInterfaces.Grids;
using HexGridInterfaces.SvgHelpers;
using HexGridInterfaces.ViewModels;

namespace SvgLib.ViewModels
{
    public class HexGridPageVM : IHexGridPageVM
    {
        private HexGridPageVM() { }

        internal HexGridPageVM(ISvgGrid grid, ISvgMap map, IHitTester hitTester)
        {
            Grid = grid;
            Map = map;
            HitTester = hitTester;
        }

        public ISvgGrid Grid { get; }

        public ISvgMap Map { get; }

        public IHitTester HitTester { get; }

    }
}