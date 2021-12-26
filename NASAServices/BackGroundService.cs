using NASANetworking;
using Shared;
using System.Net;

using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
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

        if ("video".Equals(newImageData.Media_type))
        {
            Console.WriteLine("Today was video");
            return;
        }
        
        using WebClient webClient = new WebClient();
        
        var fullPath = MakeFileAndGetFullPath("Image.jpg");

        await webClient.DownloadFileTaskAsync(newImageData.HDUrl, fullPath) ;

        setStyleOfWallpaper();
        
        SystemParametersInfo(SPI_SETDESKWALLPAPER,
            0,
            fullPath,
            SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);

       File.Delete(fullPath);
    }

    public async Task WriteExplanationAndTitle()
    {
        ImageData newImageData = await nasaClient.GetImage();

        if (string.IsNullOrEmpty(newImageData.Explanation) && string.IsNullOrEmpty(newImageData.Title))
        {
            return;
        }

        string fileName = "ExplanationFile.txt";

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
        
        string fullPath = MakeFileAndGetFullPath("ExplanationFile.txt");

        
        
        WriteToFileWithPathAndText(fullPath, newImageData.Title);
        WriteToFileWithPathAndText(fullPath, newImageData.Explanation);
    }

    private void WriteToFileWithPathAndText(string fullPath, string contentToWrite)
    {
        string[] splitForReadAbility = Regex.Split(contentToWrite, @"(?<=[.])");
        
        using TextWriter writer = new StreamWriter(fullPath, true);
        
        foreach (string stringSegment in splitForReadAbility)
        {
            writer.WriteLine(stringSegment.Trim());
        }
        writer.WriteLine(Environment.NewLine);

    }

    /**
     * Abit of a workaround, but works i guess
     */
    private string MakeFileAndGetFullPath(string fileName)
    {
        File.Create(fileName).Dispose();

        string fullPath = Path.GetFullPath(fileName);
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