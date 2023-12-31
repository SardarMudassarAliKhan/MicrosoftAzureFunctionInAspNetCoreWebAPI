using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using MicrosoftAzureFunctionInAspNetCoreWebAPI_DAL.Model;

namespace MicrosoftAzureFunctionInAspNetCoreWebAPI_BAL.IDataService
{
    public interface IDataFetcherService
    {
        Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger? log);
        Task<FetchedDataModel> FetchDataFromAPI(string apiUrl);
        Task StoreDataInBlobStorage(FetchedDataModel fetchedDataModel);
    }
}
