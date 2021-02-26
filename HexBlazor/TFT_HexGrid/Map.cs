using TFT_HexGrid.Grids;
using System.Collections.Generic;

namespace TFT_HexGrid.Maps
{
    public class Map
    {

        private Map() { }

        internal Map(Grid grid)
        {
            Hexagons = new Dictionary<int, Hexagon>(grid.Hexagons);
        }

        // maybe create this as an immutable dictionary to support undo/redo?
        public Dictionary<int, Hexagon> Hexagons { get; private set; }


        // maybe all the megagons are actually stored in here?

        // what needs to happen when hexes are added and removed from the map?

        // what do we need to do to store/export these as SVG?

        // what do we need to load one from SVG?

        // how to create a grid to overlay an arbitrary map?



    }
}
