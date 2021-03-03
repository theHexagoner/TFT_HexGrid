using Microsoft.AspNetCore.Components;
using TFT_HexGrid.SvgHelpers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace HexBlazor.Components
{
    public partial class BSvg : ComponentBase
    {

        public ElementReference Svg;

        [Parameter]
        public string WidthInches { get; set; }

        [Parameter]
        public string HeightInches { get; set; }

        [Parameter]
        public string ViewBox { get; set; }

        [Parameter]
        public string Translation { get; set; }

        [Parameter]
        public bool ShowStars { get; set; }

        public void SetGeometry(Dictionary<int, SvgHexagon> hexagons, Dictionary<int, SvgMegagon> megagons)
        {
            // compare incoming to existing

            // what's gone? - do I need to take these out of _bPolys?
            var removedHexagons = Hexagons.Where(kvp => hexagons.ContainsKey(kvp.Key) == false).Select(kvp => kvp.Key).ToArray();
            var removedMegagons = Megagons.Where(kvp => megagons.ContainsKey(kvp.Key) == false).Select(kvp => kvp.Key).ToArray();

            foreach (int id in removedHexagons) { _bPolys.Remove(id); }
            foreach (int id in removedMegagons) { _bPaths.Remove(id); }

            Hexagons = hexagons;
            Megagons = megagons;

            StateHasChanged();
        }

        #region Hexagons

        [Parameter]
        public Dictionary<int, SvgHexagon> Hexagons { get; set; } = new Dictionary<int, SvgHexagon>();

        private Dictionary<int, BPolygon> _bPolys { get; set; } = new Dictionary<int, BPolygon>();

        /// <summary>
        /// this gets called from the init for the BPolygon
        /// </summary>
        /// <param name="hex">the BPolygon to add to the local collection</param>
        internal void AddBPolygon(BPolygon hex)
        {
            if (_bPolys.ContainsKey(hex.Id) == false)
                _bPolys.Add(hex.Id, hex);
        }

        [Parameter]
        public string HexStroke { get; set; }

        [Parameter]
        public float HexStrokeWidth { get; set; }

        public async Task UpdateHexIsSelected(int id, bool isSelected)
        {
            await _bPolys[id].SetIsSelected(isSelected);
        }

        #endregion

        #region Megagons

        [Parameter]
        public Dictionary<int, SvgMegagon> Megagons { get; set; } = new Dictionary<int, SvgMegagon>();

        private Dictionary<int, BPath> _bPaths { get; set; } = new Dictionary<int, BPath>();

        /// <summary>
        /// this gets called from the init for the BPath
        /// </summary>
        /// <param name="mega">the BPath to add to the local collection</param>
        internal void AddBPath(BPath mega)
        {
            if (_bPaths.ContainsKey(mega.Id) == false)
                _bPaths.Add(mega.Id, mega);
        }

        [Parameter]
        public string MegaStroke { get; set; }

        [Parameter]
        public float MegaStrokeWidth { get; set; }

        #endregion




    }
}
