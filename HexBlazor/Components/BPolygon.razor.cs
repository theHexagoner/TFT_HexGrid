using Microsoft.AspNetCore.Components;

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

        [Parameter]
        public bool ShowStar { get; set; }

        [Parameter]
        public string HexFill { get; set; }

        [Parameter]
        public string StarFill { get; set; }

        private bool GetShowStars()
        {
            return ShowStar;
        }

        private string GetFill()
        {
            return IsSelected ? HexFill : "none"; 
        }

        private string GetStarFill()
        {
            return IsSelected ? StarFill : "none"; 
        }

    }

}
