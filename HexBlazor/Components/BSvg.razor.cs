using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using HexGridInterfaces.Structs;
using HexGridInterfaces.SvgHelpers;

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
        public SvgViewBox ViewBox { get; set; }

        [Parameter]
        public string Translation { get; set; }

        [Parameter]
        public bool ShowStars { get; set; }

        public void SetGeometry(IDictionary<int, ISvgHexagon> hexagons, IDictionary<int, SvgMegagon> megagons)
        {
            Hexagons = hexagons;
            Megagons = megagons;
            StateHasChanged();
        }

        #region Hexagons

        [Parameter]
        public IDictionary<int, ISvgHexagon> Hexagons { get; set; } = new Dictionary<int, ISvgHexagon>();

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
        public IDictionary<int, SvgMegagon> Megagons { get; set; } = new Dictionary<int, SvgMegagon>();

        [Parameter]
        public string MegaStroke { get; set; }

        [Parameter]
        public float MegaStrokeWidth { get; set; }

        #endregion




    }
}
