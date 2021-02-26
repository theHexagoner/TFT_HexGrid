using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace HexBlazor.Components
{
    public partial class BPolygon : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [Parameter]
        public string Points { get; set; }

        [Parameter]
        public string Fill { get; set; } = "#FFFFFF";

        [CascadingParameter]
        public BSvg Container { get; set; }

        protected override void OnInitialized()
        {
            Container.AddBPolygon(this);
        }

        public async Task SetFill(string fill)
        {
            Fill = fill;
            StateHasChanged();
        }
    }

}
