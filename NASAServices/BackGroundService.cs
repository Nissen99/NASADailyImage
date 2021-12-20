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
        
        var fullPath = MakeFileAndGetFullPath();

        await webClient.DownloadFileTaskAsync(newImageData.HDUrl, fullPath) ;

        setStyleOfWallpaper();
        
        SystemParametersInfo(SPI_SETDESKWALLPAPER,
            0,
            fullPath,
            SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);

       File.Delete(fullPath);
    }

    /**
     * Abit of a workaround, but works i guess
     */
    private string MakeFileAndGetFullPath()
    {
        File.Create("Image.jpg").Dispose();

        string fullPath = Path.GetFullPath("Image.jpg");
        return fullPath;
    }

    /**
     *  RegistryKey, used to set style
     * 2.ToString = Stretched
     */
    private void setStyleOfWallpaper()
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
        key.SetValue(@"WallpaperStyle", 2.ToString());
    }
}