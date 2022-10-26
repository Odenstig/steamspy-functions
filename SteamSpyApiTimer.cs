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
        private readonly string URL = "https://steamspy.com/api.php";
        private readonly string urlParameters = "?request=top100in2weeks";
        HttpClient client = new HttpClient();

        public SteamSpyApiTimer(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SteamSpyApiTimer>();
        }

        [Function("SteamSpyApiTimer")]
        [QueueOutput("games-list", Connection = "AzureWebJobsStorage")] //För storage
        public async Task<TopTenGamesList> Run([TimerTrigger("0 */5 * * * *")] MyInfo myTimer)
        {

            client.BaseAddress = new Uri(URL);
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;

            var jsonRet = await response.Content.ReadAsStringAsync();
            var allGames = JsonSerializer.Deserialize<Dictionary<string, ResponseData>>(jsonRet);

            var tenGames = allGames.OrderBy(i => i.Value.Average2weeks).Take(10);

            //Serializar så att vi kan lägga in strängen i DB senare.
            var serJson = JsonSerializer.Serialize(tenGames);

            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            //Kan plocka bort den här senare.
            _logger.LogInformation($"{serJson}");

            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");

            return new TopTenGamesList("1", DateTimeOffset.UtcNow, serJson);
        }
    }

    //Vette fan om DeviceId är nödvändigt här, kan säkert plocka bort den senare.
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
