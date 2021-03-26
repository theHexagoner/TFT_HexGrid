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
        /// Return the SvgHexagon identified by ID from th SvgHexagons collection
        /// </summary>
        /// <param name="ID">Unique key to retrieve SvgHexagon</param>
        /// <returns>ISvgHexagon instance</returns>
        /// <remarks>Throws error if ID does not exist, consider check for existing with TryGetHex before calling this method</remarks>
        ISvgHexagon GetHex(int ID);

    }
}