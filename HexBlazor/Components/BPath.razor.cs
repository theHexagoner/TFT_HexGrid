using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace HexBlazor.Components
{
    public partial class BPath : ComponentBase
    {
        // e.g. <path d="M20,230 Q40,205 50,230 T90,230" fill="none" stroke="blue" stroke-width="5"/>

        [Parameter]
        public int Id { get; set; }

        [Parameter]
        public string D { get; set; }

        //[CascadingParameter]
        //public BSvg Container { get; set; }

        //protected override void OnInitialized()
        //{
        //    Container.AddBPath(this);
        //}

        //public async Task SetD(string d)
        //{
        //    D = d;
        //    StateHasChanged();
        //}

    }
}
