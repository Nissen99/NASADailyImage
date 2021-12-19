using System.Text.Json;

namespace Shared;

public class ImageData
{
    public string Copyright { get; set; }
    public DateTime Date { get; set; }
    public string Explanation { get; set; }
    public Uri HDUrl { get; set; }
    public Uri Url { get; set; }
    public string Media_type { get; set; }
    public string Title { get; set; }


    public string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions(){WriteIndented = true});
    }
    
}