using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using MicrosoftAzureFunctionInAspNetCoreWebAPI_BAL.IDataService;
using MicrosoftAzureFunctionInAspNetCoreWebAPI_DAL.IRepository;
using MicrosoftAzureFunctionInAspNetCoreWebAPI_DAL.Model;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MicrosoftAzureFunctionInAspNetCoreWebAPI_BAL.DataFetcherService
{
    public class DataFetcherService : IDataFetcherService
    {
        private readonly IDataFetcher _repositoryDataFetcher;
        private readonly ILogger _logger;

        public DataFetcherService(IDataFetcher repositoryDataFetcher, ILogger<DataFetcherService> logger)
        {
            _repositoryDataFetcher = repositoryDataFetcher;
            _logger = logger;
        }

        public async Task<FetchedDataModel> FetchDataFromAPI(string apiUrl)
        {
            try
            {
                string jsonData = await _repositoryDataFetcher.FetchDataFromAPI(apiUrl);
                if(string.IsNullOrEmpty(jsonData))
                {
                    _logger.LogWarning("Received empty data from the API.");
                    return null;
                }

                FetchedDataModel fetchedData = JsonSerializer.Deserialize<FetchedDataModel>(jsonData);
                return fetchedData;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to fetch data from the API: {ex.Message}");
                throw;
            }
        }


        public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer,ILogger logger)
        {
            _logger.LogInformation($"DataFetcherService executed at: {DateTime.Now}");

            string apiUrl = "https://api.csharpcorner.com/Articledata"; // Replace with your API URL

            try
            {
                FetchedDataModel fetchedData = await FetchDataFromAPI(apiUrl);

                if(fetchedData != null)
                {
                    await StoreDataInBlobStorage(fetchedData);
                }
                else
                {
                    _logger.LogWarning("Failed to fetch data from the API.");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to run DataFetcherService: {ex.Message}");
                throw;
            }
        }


        public Task StoreDataInBlobStorage(FetchedDataModel fetchedDataModel)
        {
            try
            {
                return _repositoryDataFetcher.StoreDataInBlobStorage(fetchedDataModel);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to store data in Blob Storage: {ex.Message}");
                throw;
            }
        }
    }
}
