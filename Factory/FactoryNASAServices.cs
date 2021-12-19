using NASADailyImage;

namespace Factory;

public class FactoryNASAServices
{
    public static IBackGroundService GetBackGroundService()
    {
        return new BackGroundService(FactoryNASANetworking.GetNasaDailyImageClient());
    }
}