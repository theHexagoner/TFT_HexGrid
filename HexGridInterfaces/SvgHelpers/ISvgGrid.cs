using HexGridInterfaces.Structs;
using System.Collections.Generic;

namespace HexGridInterfaces.SvgHelpers
{
    public interface ISvgGrid
    {
        SvgViewBox SvgViewBox { get; }
        IEnumerable<KeyValuePair<int, ISvgHexagon>> SvgHexagons { get; }
        IEnumerable<KeyValuePair<int, SvgMegagon>> SvgMegagons { get; }
        
        /// <summary>
        /// Attempt to return an SvgHexagon identified by ID from th SvgHexagons collection
        /// </summary>
        /// <param name="ID">Unique key to retrieve SvgHexagon</param>
        /// <param name="hex">If found, the SvgHexagon retrieved</param>
        /// <returns>True if successful, false on failure</returns>
        bool TryGetHex(int? ID, out ISvgHexagon hex);

        /// <summary>
        /// set IsSelected property for hexagon
        /// </summary>
        /// <param name="ID">identifier of hexagon</param>
        void SelectHex(int ID);

        /// <summary>
        /// set IsSelected property for hexagon
        /// </summary>
        /// <param name="ID">identifier of hexagon</param>
        void DeselectHex(int ID);

    }
}