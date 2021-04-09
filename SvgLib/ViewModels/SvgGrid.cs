
using HexGridInterfaces.Structs;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SvgLib.Grids
{
    //public class SvgGrid : ISvgGrid
    //{
    //    // no default constructor
    //    private SvgGrid() { }

    //    // call this from the builder
    //    internal SvgGrid(IEnumerable<KeyValuePair<int, SvgHexagon>> svgHexagons,
    //                     IEnumerable<KeyValuePair<int, SvgMegagon>> svgMegagons,
    //                     SvgViewBox svgViewBox)
    //    {
    //        _hexDict = svgHexagons.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    //        SvgMegagons = svgMegagons;
    //        SvgViewBox = svgViewBox;
    //    }

    //    private readonly IDictionary<int, SvgHexagon> _hexDict;
        
    //    [JsonIgnore]
    //    public IEnumerable<KeyValuePair<int, SvgHexagon>> SvgHexagons 
    //    { 
    //        get
    //        {
    //            return _hexDict;
    //        }
    //    }

    //    public IEnumerable<KeyValuePair<int, SvgMegagon>> SvgMegagons { get; }

    //    public SvgViewBox SvgViewBox { get; }

    //    public bool TryGetHex(int? ID, out SvgHexagon? hex)
    //    {
    //        bool result = false;
    //        SvgHexagon? foundHex = null;
            
    //        if (ID.HasValue)
    //        {
    //            result = _hexDict.TryGetValue(ID.Value, out SvgHexagon found);
    //            if (result) foundHex = found;
    //        }
 
    //        hex = foundHex;
    //        return result;
    //    }

    //    //public void SelectHex(int ID)
    //    //{
    //    //    _hexDict.Single(kvp => kvp.Key == ID).Value.Select();
    //    //}

    //    //public void DeselectHex(int ID)
    //    //{
    //    //    _hexDict.Single(kvp => kvp.Key == ID).Value.Deselect();
    //    //}

    //    #region Json serialization/deserialization

    //    [JsonConstructor]
    //    public SvgGrid(IEnumerable<KeyValuePair<int, object>> jsonSvgHexagons,
    //                   IEnumerable<KeyValuePair<int, SvgMegagon>> svgMegagons,
    //                   SvgViewBox svgViewBox)
    //    {
    //        _hexDict = jsonSvgHexagons.ToDictionary(kvp => kvp.Key, kvp => (SvgHexagon)(kvp.Value));
    //        SvgMegagons = svgMegagons;
    //        SvgViewBox = svgViewBox;
    //    }

    //    [JsonPropertyName("SvgHexagons")]
    //    public IEnumerable<KeyValuePair<int, object>> JsonSvgHexagons => (IEnumerable<KeyValuePair<int, object>>)SvgHexagons;


    //    #endregion


    //}
}
