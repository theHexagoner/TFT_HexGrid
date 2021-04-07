using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using HexGridInterfaces.Structs;
using HexGridInterfaces.Factories;

namespace HexBlazorAF
{
    public class GetViewModel
    {
        private IHexGridPageVmBuilder _builder;

        public GetViewModel(IHexGridPageVmBuilder builder)
        {
            _builder = builder;
        }

        [FunctionName("GetViewModel")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            // parse the request body and get params for the grid
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();


            GridVars p = JsonSerializer.Deserialize<GridVars>(requestBody);

            // generate the VM from the supplied params
            var svgGrid = _builder.Build(p);

            // serialize the VM as JSON and return:
            // var jsonized = JsonSerializer.Serialize(svgGrid);

            return new OkObjectResult(svgGrid);
        }
    }
}
