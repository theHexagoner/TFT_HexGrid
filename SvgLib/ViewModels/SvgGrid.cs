﻿
using HexGridInterfaces.Structs;
using HexGridInterfaces.SvgHelpers;
using System.Collections.Generic;
using System.Linq;

namespace SvgLib.Grids
{
    public class SvgGrid : ISvgGrid
    {
        // no default constructor
        private SvgGrid() { }

        // call this from the builder
        internal SvgGrid(IEnumerable<KeyValuePair<int, ISvgHexagon>> svgHexagons,
                         IEnumerable<KeyValuePair<int, SvgMegagon>> svgMegagons,
                         SvgViewBox viewBox)
        {
            _hexDict = svgHexagons.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            SvgMegagons = svgMegagons;
            SvgViewBox = viewBox;
        }

        private readonly IDictionary<int, ISvgHexagon> _hexDict;
        public IEnumerable<KeyValuePair<int, ISvgHexagon>> SvgHexagons 
        { 
            get
            {
                return _hexDict;
            }
        }

        public IEnumerable<KeyValuePair<int, SvgMegagon>> SvgMegagons { get; }

        public SvgViewBox SvgViewBox { get; }

        public bool TryGetHex(int? ID, out ISvgHexagon hex)
        {
            bool result = false;
            ISvgHexagon foundHex = null;
            
            if (ID.HasValue)
            {
                result = _hexDict.TryGetValue(ID.Value, out foundHex);
            }
 
            hex = foundHex;
            return result;
        }

        public void SelectHex(int ID)
        {
            _hexDict.Single(kvp => kvp.Key == ID).Value.Select();
        }

        public void DeselectHex(int ID)
        {
            _hexDict.Single(kvp => kvp.Key == ID).Value.Deselect();
        }

    }
}
