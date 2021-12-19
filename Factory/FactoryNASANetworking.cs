using NASANetworking;

namespace Factory;

public class FactoryNASANetworking
{
    public static INASADailyImageClient GetNasaDailyImageClient()
    {
        return new NASADailyImageRESTClient();
    }
}