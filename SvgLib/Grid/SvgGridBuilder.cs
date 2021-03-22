using HexGridLib.Grids;
using SvgLib.Paths;
using SvgLib.Polygons;
using System.Collections.Generic;
using System.Linq;

namespace SvgLib.Grids
{
    public sealed class SvgGridBuilder
    {

        public static SvgGrid Build(Grid grid)
        {
            Dictionary<int, SvgHexagon> svgHexagons = GetSvgHexagons(grid.Hexagons.Values.ToArray());
            Dictionary<int, SvgMegagon> svgMegagons = GetSvgMegagons(grid.Edges.Values.ToArray());
            
            return new(svgHexagons, svgMegagons);
        }

        private static Dictionary<int, SvgHexagon> GetSvgHexagons(Hexagon[] hexagons)
        {
            Dictionary<int, SvgHexagon> svgHexagons = new();

            // get the SVG data for each hexagon
            foreach (Hexagon h in hexagons)
            {
                svgHexagons.Add(h.ID, new SvgHexagon(h.ID, h.Row, h.Col, h.Points));
            }

            return svgHexagons;
        }

        private static Dictionary<int, SvgMegagon> GetSvgMegagons(Edge[] edges)
        {
            Dictionary<int, SvgMegagon> svgMegagons = new();

            // build the SvgMegagons
            foreach (Edge edge in edges)
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
