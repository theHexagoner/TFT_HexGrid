using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using TFT_HexGrid.SvgHelpers;

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
        public string Translation { get; set; }

        [Parameter]
        public bool ShowStars { get; set; }

        public void SetGeometry(Dictionary<int, SvgHexagon> hexagons, Dictionary<int, SvgMegagon> megagons)
        {
            Hexagons = hexagons;
            Megagons = megagons;
            StateHasChanged();
        }

        #region Hexagons

        [Parameter]
        public Dictionary<int, SvgHexagon> Hexagons { get; set; } = new Dictionary<int, SvgHexagon>();

        [Parameter]
        public string HexStroke { get; set; }

        [Parameter]
        public float HexStrokeWidth { get; set; }

        [Parameter]
        public string HexFill { get; set; }

        public async Task UpdateHexIsSelected(int id, bool isSelected)
        {
            Hexagons[id].IsSelected = isSelected;
            await Task.Delay(1);
            StateHasChanged();
        }

        #endregion

        #region Megagons

        [Parameter]
        public Dictionary<int, SvgMegagon> Megagons { get; set; } = new Dictionary<int, SvgMegagon>();

        [Parameter]
        public string MegaStroke { get; set; }

        [Parameter]
        public float MegaStrokeWidth { get; set; }

        #endregion




    }
}
