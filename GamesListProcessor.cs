using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using functions.model;

namespace functions.queue
{
    public class GamesListProcessor
    {
        private readonly ILogger _logger;

        public GamesListProcessor(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GamesListProcessor>();
        }

        [Function("GamesListProcessor")]
        public TableData Run([QueueTrigger("games-list", Connection = "AzureWebJobsStorage")] TopTenGamesList gamesList)
        {
            try
            {
                if (gamesList.Json == null)
                {
                    _logger.LogError("Json data is null");
                    throw new ArgumentNullException();
                }

                var allGames = JsonSerializer.Deserialize<Dictionary<string, ResponseData>>(gamesList.Json);

                var tenGames = allGames.OrderByDescending(i => i.Value.Average2weeks).Take(10);

                var serJson = JsonSerializer.Serialize(tenGames);

                _logger.LogInformation($"C# Queue trigger function processed: {serJson}");

                return new TableData(
                        gamesList.DeviceId,
                        $"{(DateTimeOffset.MaxValue.Ticks - gamesList.Timestamp.Ticks):d10} - {Guid.NewGuid():N}",
                        serJson);

            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured: {ex}");
                throw;
            }
        }

        public record TableData(string PartitionKey, string RowKey, string Json);
    }
}
