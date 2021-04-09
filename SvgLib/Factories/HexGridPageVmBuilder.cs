using HexGridInterfaces.Factories;
using HexGridInterfaces.Grids;
using HexGridInterfaces.Structs;
using HexGridInterfaces.ViewModels;
using SvgLib.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SvgLib.Factories
{
    public class HexGridPageVmBuilder : IHexGridPageVmBuilder
    {
        private readonly IGridFactory _gridFactory;
        //private readonly ISvgGridBuilder _svgGridBuilder;
        //private readonly ISvgMapBuilder _svgMapBuilder;
        //private readonly IHitTesterFactory _hitTesterFactory;

        // for DI
        public HexGridPageVmBuilder(IGridFactory gridFactory) //, ISvgGridBuilder svgGridBuilder ISvgMapBuilder svgMapBuilder, IHitTesterFactory hitTesterFactory) 
        {
            _gridFactory = gridFactory;
            //_svgGridBuilder = svgGridBuilder;
            //_svgMapBuilder = svgMapBuilder;
            //_hitTesterFactory = hitTesterFactory;
        }

        public IHexGridPageVM Build(GridVars vars)
        {
            // create a grid with the builder passed in by DI?
            IGrid grid = _gridFactory.Build(vars.RowCount, vars.ColCount, vars.Radius, vars.Origin, vars.Schema);
            //IMap map = grid.InitMap();

            IDictionary<int, SvgHexagon> svgHexagons = GetSvgHexagons(grid.Hexagons.Values.ToArray());
            IDictionary<int, SvgMegagon> svgMegagons = GetSvgMegagons(grid.Edges.Values.ToArray());

            SvgGrid svgGrid = new SvgGrid(svgHexagons, svgMegagons, vars.ViewBox);
            //ISvgMap svgMap = _svgMapBuilder.Build(map, vars.ViewBox);

            //IEnumerable<int> hexagonIDs = grid.Hexagons.Keys;

            //IHitTester hitTester = _hitTesterFactory.Build(vars.RowCount, vars.ColCount, vars.Radius, vars.Origin, vars.Schema, hexagonIDs);

            return new HexGridPageVM(svgGrid); //, svgMap, hitTester);
        }

        private static IDictionary<int, SvgHexagon> GetSvgHexagons(IHexagon[] hexagons)
        {
            Dictionary<int, SvgHexagon> svgHexagons = new Dictionary<int, SvgHexagon>();

            // get the SVG data for each hexagon
            foreach (IHexagon h in hexagons)
            {
                svgHexagons.Add(h.ID, new SvgHexagon(h.ID, h.Row, h.Col, GetHexPoints(h.Points), GetCenterD()));
            }

            return svgHexagons;
        }

        private static IDictionary<int, SvgMegagon> GetSvgMegagons(IEdge[] edges)
        {
            Dictionary<int, SvgMegagon> svgMegagons = new Dictionary<int, SvgMegagon>();

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

        private static string GetHexPoints(GridPoint[] points)
        {
            return string.Join(" ", points.Select(p => string.Format("{0},{1}", p.X, p.Y)));
        }

        private static string GetCenterD()
        {
            return string.Empty;
        }

    }
}
