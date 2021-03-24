using HexGridInterfaces.Grids;
using HexGridInterfaces.Structs;
using HexGridInterfaces.SvgHelpers;
using HexGridInterfaces.Factories;
using SvgLib.Polygons;
using System.Collections.Generic;
using System.Linq;

namespace SvgLib.Grids
{
    public sealed class SvgGridBuilder : ISvgGridBuilder
    {
        private readonly IGridFactory _gridFactory;

        public SvgGridBuilder(IGridFactory gridFactory) { _gridFactory = gridFactory; }

        public ISvgGrid Build(int rowCount, int colCount, GridPoint size, GridPoint origin, OffsetSchema schema, SvgViewBox viewBox)
        {
            // create a grid with the builder passed in by DI?
            IGrid grid = _gridFactory.Build(rowCount, colCount, size, origin, schema);

            Dictionary<int, ISvgHexagon> svgHexagons = GetSvgHexagons(grid.Hexagons.Values.ToArray());
            Dictionary<int, SvgMegagon> svgMegagons = GetSvgMegagons(grid.Edges.Values.ToArray());
            
            return new SvgGrid(svgHexagons, svgMegagons, viewBox);
        }

        private static Dictionary<int, ISvgHexagon> GetSvgHexagons(IHexagon[] hexagons)
        {
            Dictionary<int, ISvgHexagon> svgHexagons = new();

            // get the SVG data for each hexagon
            foreach (IHexagon h in hexagons)
            {
                svgHexagons.Add(h.ID, new SvgHexagon(h.ID, h.Row, h.Col, h.Points));
            }

            return svgHexagons;
        }

        private static Dictionary<int, SvgMegagon> GetSvgMegagons(IEdge[] edges)
        {
            Dictionary<int, SvgMegagon> svgMegagons = new();

            // build the SvgMegagons
            foreach (IEdge edge in edges)
            {
                if (edge.GetIsMegaLine())
                {
                    // add a new SvgMegagon
                    svgMegagons.Add(edge.ID, new SvgMegagon(edge.ID, edge.PathD));
                }
            }

            return svgMegagons;
        }

    }
}
