using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using HexGridInterfaces.Structs;
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
        public string BackgroundFill { get; set; }

        [Parameter]
        public string ViewBox { get; set; }

        [Parameter]
        public double RectOriginX { get; set; }

        [Parameter]
        public double RectOriginY { get; set; }

        [Parameter]
        public string Translation { get; set; }

        [Parameter]
        public bool ShowStars { get; set; }

        public void SetGeometry(IEnumerable<KeyValuePair<int, SvgHexagon>> hexagons, IEnumerable<KeyValuePair<int, SvgMegagon>> megagons)
        {
            Hexagons = hexagons.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            Megagons = megagons.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            StateHasChanged();
        }

        #region Hexagons

        public IDictionary<int, SvgHexagon> Hexagons { get; set; } = new Dictionary<int, SvgHexagon>();

        [Parameter]
        public string HexStroke { get; set; }

        [Parameter]
        public float HexStrokeWidth { get; set; }

        [Parameter]
        public string HexFill { get; set; }

        public void SelectHex(int id)
        {
            //Hexagons[id].Select();
            //StateHasChanged();
        }

        public void DeselectHex(int id)
        {
            //Hexagons[id].Deselect();
            //StateHasChanged();
        }

        #endregion

        #region Megagons

        public IDictionary<int, SvgMegagon> Megagons { get; set; } = new Dictionary<int, SvgMegagon>();

        [Parameter]
        public string MegaStroke { get; set; }

        [Parameter]
        public float MegaStrokeWidth { get; set; }

        #endregion




    }
}
