using Shared;

namespace NASANetworking;

public interface INASADailyImageClient
{


    Task<ImageData> GetImage();

}