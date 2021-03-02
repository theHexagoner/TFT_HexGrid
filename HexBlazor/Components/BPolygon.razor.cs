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
        public bool IsSelected { get; set; }

        [Parameter]
        public string StarD { get; set; }

        public bool GetShowStars()
        {
            return Container.ShowStars;
        }

        private string GetFill()
        {
            return IsSelected ? "#FFFFFF" : "none";
        }

        private string GetStarFill()
        {
            return IsSelected ? Container.HexStroke : "none";
        }


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
