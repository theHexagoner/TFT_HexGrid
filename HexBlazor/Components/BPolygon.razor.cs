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

        private string GetFill()
        {
            return IsSelected ? "#FFFFFF" : "none";
        }

        [Parameter]
        public bool IsSelected { get; set; }

        [CascadingParameter]
        public BSvg Container { get; set; }

        protected override void OnInitialized()
        {
            Container.AddBPolygon(this);
        }

        public async Task SetIsSelected(bool isSelected)
        {
            IsSelected = isSelected;
            StateHasChanged();
        }

    }

}
