using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace HexGridInterfaces.Structs
{
    public class SvgGrid
    {
        [JsonConstructor]
        public SvgGrid(IEnumerable<KeyValuePair<int, SvgHexagon>> svgHexagons,
               IEnumerable<KeyValuePair<int, SvgMegagon>> svgMegagons,
               SvgViewBox svgViewBox)
        {
            _hexDict = svgHexagons.ToDictionary(kvp => kvp.Key, kvp => (SvgHexagon)(kvp.Value));
            SvgMegagons = svgMegagons;
            SvgViewBox = svgViewBox;
        }

        [JsonIgnore]
        private readonly IDictionary<int, SvgHexagon> _hexDict;

        [JsonInclude]
        public IEnumerable<KeyValuePair<int, SvgHexagon>> SvgHexagons
        {
            get
            {
                return _hexDict;
            }
        }

        [JsonInclude]
        public IEnumerable<KeyValuePair<int, SvgMegagon>> SvgMegagons { get; }

        [JsonInclude]
        public SvgViewBox SvgViewBox { get; }

        public bool TryGetHex(int? ID, out SvgHexagon? hex)
        {
            bool result = false;
            SvgHexagon? foundHex = null;

            if (ID.HasValue)
            {
                result = _hexDict.TryGetValue(ID.Value, out SvgHexagon found);
                if (result) foundHex = found;
            }

            hex = foundHex;
            return result;
        }

    }
}
