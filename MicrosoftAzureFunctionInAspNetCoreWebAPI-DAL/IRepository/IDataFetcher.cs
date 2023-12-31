using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using MicrosoftAzureFunctionInAspNetCoreWebAPI_DAL.Model;

namespace MicrosoftAzureFunctionInAspNetCoreWebAPI_DAL.IRepository
{
    public interface IDataFetcher
    {
        Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log);
        Task<string> FetchDataFromAPI(string apiUrl);
        Task StoreDataInBlobStorage(FetchedDataModel fetchedDataModel);
    }
}
