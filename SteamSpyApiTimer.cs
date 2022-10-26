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
        public async Task Run([TimerTrigger("0 */5 * * * *")] MyInfo myTimer)
        {

            client.BaseAddress = new Uri(URL);
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;

            var jsonRet = await response.Content.ReadAsStringAsync();
            var allGames = JsonSerializer.Deserialize<Test>(jsonRet);

            // var tenGames = allGames.OrderBy(i => i.average_2weeks).Take(10);


            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"{allGames.TestList}");
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
        }
    }

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
