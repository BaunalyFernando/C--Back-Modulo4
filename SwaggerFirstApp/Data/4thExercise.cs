namespace SwaggerFirstApp.Data
{
    public class _4thExercise
{
    //I use httpClient to have the possibility to handle de APIS.
    private readonly HttpClient _httpClient;

    public _4thExercise()
    {
        // I instantiate the httpClient
        _httpClient = new HttpClient();
    }

    //public async Task<T> GetDataFromAPI(string url)
    //{
    //    // Implementation of Polly, to handle the retries and the policy.
    //    AsyncRetryPolicy retryPolicy = Policy
    //    // here we can have the possibility to handle an operation with an exception.
    //    .Handle<HttpRequestException>()
    //    .Or<TaskCanceledException>()
    //    .WaitAndRetryAsync(
    //        // here the operation will try 3 times in order to handle the error, and the time between them will increase in each try.
    //        retryCount: 3,
    //        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
    //        onRetry: (exception, timeSpan, retryCount, context) =>
    //        {

    //            Console.WriteLine($"Retry {retryCount} after {timeSpan.Seconds} seconds due to: {exception.Message}");
    //        });

    //    try
    //    {
    //        // here will call to the api and get the data.
    //        HttpResponseMessage response = await _httpClient.GetAsync(url);
    //        // if something goes wrong like a 400 error, then the system will throw an error.
    //        if (!response.IsSuccessStatusCode)
    //        {
    //            throw new HttpRequestException($"Request failed");
    //        }

    //        // here it will analyze the data and ream from json file.

    //        T data = await response.Content.ReadFromJsonAsync<T>();

    //        return data;
    //    }
    //    catch
    //    {
    //        Console.WriteLine($"Error occurred trying to fetch data from the API");
    //        throw;
    //    }
    //}
}

}
