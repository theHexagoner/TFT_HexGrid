using HexGridInterfaces.Factories;
using HexGridInterfaces.Structs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HexBlazorAF
{
    public class GetViewModel
    {
        private readonly IHexGridPageVmBuilder _builder;

        public GetViewModel(IHexGridPageVmBuilder builder)
        {
            _builder = builder;
        }

        [FunctionName("GetViewModel")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            // parse the request body and get params for the grid
            GridVars p = await JsonSerializer.DeserializeAsync<GridVars>(req.Body);

            // generate the VM from the supplied params
            var vm = _builder.Build(p);

            // serialize the VM as JSON and return:
            var json = JsonSerializer.Serialize(vm);

            return new OkObjectResult(json);
        }
    }
}
