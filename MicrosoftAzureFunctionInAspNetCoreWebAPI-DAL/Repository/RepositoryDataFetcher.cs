using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MicrosoftAzureFunctionInAspNetCoreWebAPI_DAL.IRepository;
using MicrosoftAzureFunctionInAspNetCoreWebAPI_DAL.Model;
using System.Text.Json;

namespace MicrosoftAzureFunctionInAspNetCoreWebAPI_DAL.Repository
{
    public class RepositoryDataFetcher : IDataFetcher
    {
        private static readonly HttpClient httpClient;
        public RepositoryDataFetcher(HttpClient httpClient)
        {
            httpClient = httpClient ?? new HttpClient();
        }

        public async Task<string> FetchDataFromAPI(string apiUrl)
        {
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task StoreDataInBlobStorage(FetchedDataModel fetchedDataModel)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("YourStorageConnectionString");
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("data-container");
            await container.CreateIfNotExistsAsync();

            CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{fetchedDataModel.Id}.json");
            using(MemoryStream ms = new MemoryStream())
            {
                JsonSerializer.SerializeAsync(ms, fetchedDataModel);
                ms.Position = 0;
                await blockBlob.UploadFromStreamAsync(ms);
            }
        }

        public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"DataFetcherFunction executed at: {DateTime.Now}");

            string apiUrl = "https://api.csharpcorner.com/Articledata"; // Replace with your API URL

            string dataJson = await FetchDataFromAPI(apiUrl);

            FetchedDataModel fetchedData = JsonSerializer.Deserialize<FetchedDataModel>(dataJson);

            await StoreDataInBlobStorage(fetchedData);
        }
    }
}
