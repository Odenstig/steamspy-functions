using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using functions.model;


namespace functions
{
    public class SteamSpyApiTimer
    {
        private readonly ILogger _logger;
        private readonly string _URL;
        private readonly string _urlParameters;
        HttpClient client = new HttpClient();

        public SteamSpyApiTimer(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SteamSpyApiTimer>();
            _URL = "https://steamspy.com/api.php";
            _urlParameters = "?request=top100in2weeks";
        }

        [Function("SteamSpyApiTimer")]
        [QueueOutput("games-list", Connection = "AzureWebJobsStorage"),] //För storage
        public async Task<TopTenGamesList> Run([TimerTrigger("0 */5 * * * *")] MyInfo myTimer)
        {
            try
            {
                client.BaseAddress = new Uri(_URL);
                HttpResponseMessage response = client.GetAsync(_urlParameters).Result;

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to get Json Data");
                    throw new HttpRequestException($"Failed to get Json Data, Error code: {response.StatusCode}");
                }

                var jsonRet = await response.Content.ReadAsStringAsync();

                _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");

                return new TopTenGamesList("1", DateTimeOffset.UtcNow, jsonRet);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error has occured: {ex}");
                throw;
            }
        }
    }

    public record TopTenGamesList(string DeviceId, DateTimeOffset Timestamp, string Json);

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
