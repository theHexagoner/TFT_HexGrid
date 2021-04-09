using HexGridInterfaces.Structs;
using HexGridInterfaces.ViewModels;
using System.Text.Json.Serialization;

namespace SvgLib.ViewModels
{
    public class HexGridPageVM : IHexGridPageVM
    {
        private HexGridPageVM() { }

        [JsonConstructor]
        public HexGridPageVM(SvgGrid grid)//, ISvgMap map, IHitTester hitTester)
        {
            Grid = grid;
            //Map = map;
            //HitTester = hitTester;
        }

        [JsonInclude]
        public SvgGrid Grid { get; }

    }
}