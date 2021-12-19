using NASANetworking;
using Shared;
using System.Net;

using System.Runtime.InteropServices;
using Microsoft.Win32;


namespace NASADailyImage;

public class BackGroundService : IBackGroundService
{
    
    private const int SPI_SETDESKWALLPAPER = 20;
    private const int SPIF_UPDATEINIFILE = 0x01;
    private const int SPIF_SENDWININICHANGE = 0x02;
    //Absolut path, could be made relative 
    private const string FILE_PATH = @"C:\Users\Mikkel\RiderProjects\NASADailyImage\NASADailyImage\Util\Image.jpg";
    
    private INASADailyImageClient nasaClient;
    public BackGroundService(INASADailyImageClient nasaDailyImageClient)
    {
        nasaClient = nasaDailyImageClient;
    }
    
    /**
     * https://stackoverflow.com/questions/1061678/change-desktop-wallpaper-using-code-in-net
     */
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

    /**
     * https://stackoverflow.com/questions/1061678/change-desktop-wallpaper-using-code-in-net
     *
     * RegistryKey, used to set style, 2.ToString = Stretched
     */
    public async Task GetImage()
    {
        ImageData newImageData = await nasaClient.GetImage();

        using WebClient webClient = new WebClient();

        await webClient.DownloadFileTaskAsync(newImageData.HDUrl, FILE_PATH) ;
    
        
        //RegistryKey, used to set style, 2.ToString = Stretched
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
        key.SetValue(@"WallpaperStyle", 2.ToString());
        
        SystemParametersInfo(SPI_SETDESKWALLPAPER,
            0,
            FILE_PATH,
            SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);

    }
    
}