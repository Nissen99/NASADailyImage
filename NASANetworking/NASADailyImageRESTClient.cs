using Shared;

namespace NASANetworking;

public class NASADailyImageRESTClient : HttpClientBase, INASADailyImageClient
{
    public async Task<ImageData> GetImage()
    {
        HttpClient httpClient = new HttpClient();

        string request = UriToNASA + $"apod?api_key={Credentials.ApiKey}";

        HttpResponseMessage responseMessage = await httpClient.GetAsync(request);
        
        ImageData imageFromNasaApi = await HandleResponseGet<ImageData>(responseMessage);

        return imageFromNasaApi;
    }
}