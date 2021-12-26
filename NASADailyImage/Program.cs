using Factory;
using NASADailyImage;

IBackGroundService backGroundService = FactoryNASAServices.GetBackGroundService();


await backGroundService.GetImage();
await backGroundService.WriteExplanationAndTitle();
