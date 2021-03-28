using HexGridInterfaces.Grids;
using HexGridInterfaces.Structs;
using System.Collections.Generic;
using System.Linq;

namespace SvgLib.ViewModels
{
    public class HitTester : IHitTester
    {
        private readonly HexLayout _layout;
        private readonly IEnumerable<int?> _hexagonIDs;
        private readonly OffsetSchema _schema;
        private readonly int _rowCount;
        private readonly int _colCount;

        private HitTester() { }

        internal HitTester(int rowCount, int colCount, GridPoint radius, GridPoint origin, OffsetSchema schema, IEnumerable<int> hexagonIDs)
        {
            _rowCount = rowCount;
            _colCount = colCount;
            _schema = schema;
            _layout = new HexLayout(_schema.HexStyle, radius, origin);
            _hexagonIDs = hexagonIDs.Cast<int?>();
        }

        /// <summary>
        /// return the ID for the hexagon that contains the supplied GridPoint 
        /// </summary>
        /// <param name="point">the cartesian coordiates to check</param>
        /// <returns>System.Int32 containing the ID of the selected hexagon, or null</returns>
        public int? HitTest(GridPoint point)
        {
            // turn the point into a CubeF
            // round the CubeF to get the Cubic coordinates
            var cube = _layout.PointToCubeF(point).Round();
            int hash = GetUniqueID(cube);

            // get the Hexagon from the hash of the cubic coordinates
            var hex = _hexagonIDs.FirstOrDefault(h => h == hash);

            if (hex != null)
                return hex;

            return null;
        }

        private int GetUniqueID(CubicCoordinate cube)
        {
            return cube.GetUniqueID(_schema.HexStyle, _schema.OffsetPush, _schema.MegahexSkew, _rowCount, _colCount);
        }

    }
}
