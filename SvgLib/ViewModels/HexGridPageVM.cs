using HexGridInterfaces.Grids;
using HexGridInterfaces.SvgHelpers;
using HexGridInterfaces.ViewModels;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public ISvgGrid Grid { get; }

        [JsonIgnore]
        public ISvgMap Map { get; }

        [JsonIgnore]
        public IHitTester HitTester { get; }

        #region Json serialization/deserialization

        [JsonInclude]
        [JsonPropertyName("Grid")]
        public object JsonGrid => Grid;

        [JsonInclude]
        [JsonPropertyName("Map")]
        public object JsonMap => Map;

        [JsonInclude]
        [JsonPropertyName("HitTester")]
        public object JsonHitTester => HitTester;

        [JsonConstructor]
        public HexGridPageVM(object jsongrid, object jsonmap, object jsonhitTester)
        {
            // check types?

            Grid = (ISvgGrid)jsongrid;
            Map = (ISvgMap)jsonmap;
            HitTester = (IHitTester)jsonhitTester;
        }

        #endregion


    }
}