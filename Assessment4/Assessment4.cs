using Newtonsoft.Json;

namespace Assessment4
{
    public class Assessment4
    {
        private static readonly TimeSpan _period = TimeSpan.FromMinutes(1);
        private static Task _currentExecution;
        private static readonly object _lock = new();

        public static async Task RunPeriodic()
        {
            Console.WriteLine("Starting periodic execution for weather check.");

            while (true)
            {
                await ExecutePeriodicallyAsync();
            }
        }

        private static async Task ExecutePeriodicallyAsync()
        {
            lock (_lock)
            {
                if (_currentExecution is { IsCompleted: false })
                {
                    Console.WriteLine("Previous execution is still running, skipping this period.");
                    return;
                }

                _currentExecution = RunAssessmentAsync();
            }

            try
            {
                await _currentExecution;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Execution failed: {ex.Message}");
            }
            finally
            {
                await Task.Delay(_period);
            }
        }

        private static async Task RunAssessmentAsync()
        {
            try
            {
                var response = await GetFirstSuccessfulResponseAsync();
                if (response is not null)
                {
                    Console.WriteLine($"Received valid response with id: {response.id}");
                }
                else
                {
                    Console.WriteLine("No valid response received.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during run: {ex.Message}");
            }
        }

        public static async Task<RootObject> GetFirstSuccessfulResponseAsync()
        {
            using var httpClient = new HttpClient();
            using var cts = new CancellationTokenSource();
            var tasks = Enumerable.Range(1, 10)
                .Select(x => MakeHttpRequestAsync(httpClient, x, cts.Token))
                .ToList();

            try
            {
                while (tasks.Any())
                {
                    var completedTask = await Task.WhenAny(tasks);
                    tasks.Remove(completedTask);

                    try
                    {
                        var response = await completedTask;
                        if (response != null)
                        {
                            cts.Cancel();

                            //ensure graceful cleanup
                            await Task.WhenAll(tasks);

                            return response;
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        //task canceled
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Task failed with message: {ex.Message}");
                    }
                }
            }
            finally
            {
                foreach (var task in tasks)
                {
                    try
                    {
                        await task;
                    }
                    catch
                    {
                        //ignore possible exceptions
                    }
                }
            }

            return null;
        }

        static async Task<RootObject> MakeHttpRequestAsync(HttpClient httpClient, int index, CancellationToken ct)
        {
            const string url = "http://api.openweathermap.org/data/2.5/weather?q=London,UK&appid=b90bcbf24c7ba1e7aae0969e5af38ae8";
            RootObject result = null;

            try
            {
                var response = await httpClient.GetAsync(url, ct);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync(ct);
                result = JsonConvert.DeserializeObject<RootObject>(json);

                result.id = index;

                return result;
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Request {url} failed: {ex.Message}");
            }

            return null;
        }
    }
}
