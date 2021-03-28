using HexGridInterfaces.Factories;
using HexGridInterfaces.Grids;
using HexGridInterfaces.Structs;
using HexGridInterfaces.SvgHelpers;
using HexGridInterfaces.ViewModels;
using SvgLib.Polygons;
using SvgLib.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SvgLib.Factories
{
    public class HexGridPageVmBuilder : IHexGridPageVmBuilder
    {
        private readonly IGridFactory _gridFactory;
        private readonly ISvgGridBuilder _svgGridBuilder;
        private readonly ISvgMapBuilder _svgMapBuilder;
        private readonly IHitTesterFactory _hitTesterFactory;

        public HexGridPageVmBuilder(IGridFactory gridFactory, ISvgGridBuilder svgGridBuilder, ISvgMapBuilder svgMapBuilder, IHitTesterFactory hitTesterFactory) 
        {
            _gridFactory = gridFactory;
            _svgGridBuilder = svgGridBuilder;
            _svgMapBuilder = svgMapBuilder;
            _hitTesterFactory = hitTesterFactory;
        }

        public IHexGridPageVM Build(GridVars vars)
        {
            // create a grid with the builder passed in by DI?
            IGrid grid = _gridFactory.Build(vars.RowCount, vars.ColCount, vars.Radius, vars.Origin, vars.Schema);
            IMap map = grid.InitMap();

            IDictionary<int, ISvgHexagon> svgHexagons = GetSvgHexagons(grid.Hexagons.Values.ToArray());
            IDictionary<int, SvgMegagon> svgMegagons = GetSvgMegagons(grid.Edges.Values.ToArray());

            ISvgGrid svgGrid = _svgGridBuilder.Build(svgHexagons, svgMegagons, vars.ViewBox);
            ISvgMap svgMap = _svgMapBuilder.Build(map, vars.ViewBox);

            IEnumerable<int> hexagonIDs = grid.Hexagons.Keys;

            IHitTester hitTester = _hitTesterFactory.Build(vars.RowCount, vars.ColCount, vars.Radius, vars.Origin, vars.Schema, hexagonIDs);

            return new HexGridPageVM(svgGrid, svgMap, hitTester);
        }

        private static IDictionary<int, ISvgHexagon> GetSvgHexagons(IHexagon[] hexagons)
        {
            Dictionary<int, ISvgHexagon> svgHexagons = new();

            // get the SVG data for each hexagon
            foreach (IHexagon h in hexagons)
            {
                svgHexagons.Add(h.ID, new SvgHexagon(h.ID, h.Row, h.Col, h.Points));
            }

            return svgHexagons;
        }

        private static IDictionary<int, SvgMegagon> GetSvgMegagons(IEdge[] edges)
        {
            Dictionary<int, SvgMegagon> svgMegagons = new();

            // build the SvgMegagons
            foreach (IEdge edge in edges)
            {
                if (edge.IsMegaLine)
                {
                    // add a new SvgMegagon
                    svgMegagons.Add(edge.ID, new SvgMegagon(edge.ID, edge.PathD));
                }
            }

            return svgMegagons;
        }

    }
}
