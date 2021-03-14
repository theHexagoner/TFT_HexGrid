using Microsoft.AspNetCore.Components;

namespace HexBlazor.Components
{
    public partial class BPolygon : ComponentBase
    {
        [CascadingParameter]
        public BSvg Container { get; set; }
        
        [Parameter]
        public int Id { get; set; }

        [Parameter]
        public string Points { get; set; }

        [Parameter]
        public bool IsSelected { get; set; }

        [Parameter]
        public string StarD { get; set; }

        private bool GetShowStars()
        {
            return Container.ShowStars;
        }

        private string GetFill()
        {
            return IsSelected ? Container.HexFill : "none";
        }

        private string GetStarFill()
        {
            return IsSelected ? Container.HexStroke : "none";
        }


    }

}
