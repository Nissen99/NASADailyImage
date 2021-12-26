using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace NASANetworking;

public class HttpClientBase
{
    protected string UriToNASA = "https://api.nasa.gov/planetary/";
    
    protected async Task<T> HandleResponseGet<T>(HttpResponseMessage responseMessage)
    { 
        CheckForBadStatusCode(responseMessage);
            
        return await responseMessage.Content.ReadFromJsonAsync<T>();
    }
    
    protected StringContent FromObjectToStringContentCamelCase<T>(T toStringContent)
    {
        string toJson = JsonSerializer.Serialize(toStringContent,
            new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
            

        return new StringContent(toJson, Encoding.UTF8, "application/json");
    }

    public void CheckForBadStatusCode(HttpResponseMessage responseMessage)
    {

        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {responseMessage.StatusCode}, " +
                                $"{responseMessage.ReasonPhrase}");
        }
    }

    protected void HandleResponseNoReturn(HttpResponseMessage responseMessage)
    {
        CheckForBadStatusCode(responseMessage);
    }
}