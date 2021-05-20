# WkHtml2Image
Html to Image converter (based on wkhtmltox)

## Install
```
> Install-Package WkHtml2Image
```

## Using on API Controller

#### Startup.cs

```csharp
public void ConfigureServices(IServiceCollection services)
{
    //....
    services.AddWkHtmlToImage(options => { options.UseGraphics = false; });
    //....
}
```

#### Controller class file
```csharp
private readonly IWkHtml2ImageConverter _wkHtml2ImageConverter;

public TestController(IWkHtml2ImageConverter wkHtml2ImageConverter)
{
    _wkHtml2ImageConverter = wkHtml2ImageConverter;
}
```


#### Using with URL
```csharp
var result = await _wkHtml2ImageConverter.Convert(new Uri("http://www.google.com/"), ImageFormat.Png);
```

#### Using with HTML Content
```csharp
var sampleContent = @"<html>
                  <head></head>
                  <body>
                      <div style='margin: 0 auto; width: 400px; height:400px; background-color:yellow; text-align:center; font-size:18pt;'>Hello</div>
                  </body>
                 </html>";

var result = await _wkHtml2ImageConverter.Convert(sampleContent, ImageFormat.Png);
```

You can also use ConvertionSetting class for more detailed configurations listed below;
- Cropping(Top, Left, Width, Height)
- Transparancy
- Screen Width & Height
- Quality


If you want to use diffrent wkhtml2x libraries you can download here(https://wkhtmltopdf.org/downloads.html). You should add libraries in your project like these;
```
/SolutionFolder
   /ProjectFolder
     /lib
       /32 Bit
          /libwkhtmltox.dll
          /libwkhtmltox.dylib
          /libwkhtmltox.so
       /64 Bit
          /libwkhtmltox.dll
          /libwkhtmltox.dylib
          /libwkhtmltox.so
```
