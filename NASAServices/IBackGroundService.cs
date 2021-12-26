namespace NASADailyImage;

public interface IBackGroundService
{
    Task GetImage();
    Task WriteExplanationAndTitle();
}