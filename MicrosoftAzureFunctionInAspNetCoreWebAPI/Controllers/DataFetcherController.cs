using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using MicrosoftAzureFunctionInAspNetCoreWebAPI_BAL.IDataService;

namespace MicrosoftAzureFunctionInAspNetCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataFetcherController : ControllerBase
    {
        private readonly IDataFetcherService _dataFetcherService;
        private readonly ILogger<DataFetcherController> _logger;

        public DataFetcherController(IDataFetcherService dataFetcherService, ILogger<DataFetcherController> logger)
        {
            _dataFetcherService = dataFetcherService;
            _logger = logger;
        }

        [HttpPost(nameof(FetchAndStoreData))]
        public async Task<IActionResult> FetchAndStoreData([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
        {
            try
            {
                await _dataFetcherService.Run(myTimer,null);

                return Ok("Data fetched and stored successfully!");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to fetch and store data: {ex.Message}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}

