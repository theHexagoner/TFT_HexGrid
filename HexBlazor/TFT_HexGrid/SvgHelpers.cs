
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFT_HexGrid.Grids;

namespace TFT_HexGrid.SvgHelpers
{

    public struct SvgHexagon
    {
        public readonly int Id;
        public readonly string Points;

        public SvgHexagon(int id, GridPoint[] points)
        {
            Id = id;
            Points = string.Join(" ", points.Select(p => string.Format("{0},{1}", p.X, p.Y)));
        }

    }

    public struct SvgMegagon
    {
        public readonly int Id;
        public readonly string D; //e.g.path d = "M20,230 Q40,205 50,230 T90,230"

        public SvgMegagon(int id, string d)
        {
            Id = id;
            D = d;
        }

    }

}
