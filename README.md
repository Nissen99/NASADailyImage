# NASADailyImage

This project is made to use the NASA APOD api, to change the desktop background to a new astronomy picture every day. 
(https://api.nasa.gov/)

To run a key to nasa api needed, you can generate your own at: https://api.nasa.gov/ . 
Put this key in a new class made under NASANetworking
```
namespace NASANetworking;

public class Credentials
{
    public const string ApiKey = "Your_Key_Here";
    }
```

There is also a relative path in BackGroundService, here create a jpg file and put in the path
