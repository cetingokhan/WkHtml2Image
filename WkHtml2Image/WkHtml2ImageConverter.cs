using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using WkHtml2Image.Engine;
using WkHtml2Image.Engine.Settings;

namespace WkHtml2Image
{
    public class WkHtml2ImageConverter : IWkHtml2ImageConverter
    {

        private static ImageConverterTools _imageConverterTools = new ImageConverterTools();
        private static WkHtml2ImageConverterOptions _options;
        private readonly ILogger<WkHtml2ImageConverter> _logger;

        public WkHtml2ImageConverter(IOptions<WkHtml2ImageConverterOptions> options, ILogger<WkHtml2ImageConverter> logger)
        {
            _options = options.Value;
            _logger = logger;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var architectureFolder = (IntPtr.Size == 8) ? "64 bit" : "32 bit";

                var paths = Directory.GetFiles(Directory.GetCurrentDirectory(), "libwkhtmltox.dll", SearchOption.AllDirectories).Where(x => x.Contains(architectureFolder)).SingleOrDefault();

                new CustomAssemblyLoadContext().LoadUnmanagedLibrary(paths);
            }
        }

        public WkHtml2ImageConverter(WkHtml2ImageConverterOptions options, ILogger<WkHtml2ImageConverter> logger)
        {
            _options = options;
            _logger = logger;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var architectureFolder = (IntPtr.Size == 8) ? "64 bit" : "32 bit";

                var paths = Directory.GetFiles(Directory.GetCurrentDirectory(), "libwkhtmltox.dll", SearchOption.AllDirectories).Where(x => x.Contains(architectureFolder)).SingleOrDefault();

                new CustomAssemblyLoadContext().LoadUnmanagedLibrary(paths);
            }
        }

        public async Task<byte[]> Convert(string htmlContent, ImageFormat imageFormat)
        {
            return await Convert(htmlContent, imageFormat, new ConvertionSetting()
            {
                SmartWidth = true,
                Quality = 100
            });
        }

        public async Task<byte[]> Convert(Uri url, ImageFormat imageFormat)
        {
            return await Convert(url, imageFormat, new ConvertionSetting()
            {
                SmartWidth = true,
                Quality = 100
            });
        }

        public async Task<byte[]> Convert(Uri url, ImageFormat imageFormat, ConvertionSetting convertionSetting)
        {
            var result = Convert(new ImageSettings()
            {
                CropHeight = convertionSetting.CropHeight,
                CropLeft = convertionSetting.CropLeft,
                CropTop = convertionSetting.CropTop,
                CropWidth = convertionSetting.CropWidth,
                ScreenHeight = convertionSetting.ScreenHeight,
                ScreenWidth = convertionSetting.ScreenWidth,
                Format = imageFormat.ToString().ToLower(),
                SmartWidth = convertionSetting.SmartWidth,
                LogLevel = 4,
                Quality = convertionSetting.Quality,
                UrlOrInputFile = url.AbsoluteUri
            }, string.Empty);

            return await Task.FromResult(result);
        }

        public async Task<byte[]> Convert(string htmlContent, ImageFormat imageFormat, ConvertionSetting convertionSetting)
        {
            var result = Convert(new ImageSettings()
            {
                CropHeight = convertionSetting.CropHeight,
                CropLeft = convertionSetting.CropLeft,
                CropTop = convertionSetting.CropTop,
                CropWidth = convertionSetting.CropWidth,
                ScreenHeight = convertionSetting.ScreenHeight,
                ScreenWidth = convertionSetting.ScreenWidth,
                Format = imageFormat.ToString().ToLower(),
                SmartWidth = convertionSetting.SmartWidth,
                LogLevel = 4,
                Quality = convertionSetting.Quality
            }, htmlContent);

            return await Task.FromResult(result);
        }

        private static Mutex _lock = new Mutex(true, "WkHtml2Image");
        private byte[] Convert(ImageSettings imageSettings, string htmlContent)
        {
            _lock.WaitOne();
            try
            {
                /*
                https://wkhtmltopdf.org/libwkhtmltox/

                To image c-bindings
                The file image.h contains a fairly high level and stable pure c binding to wkhtmltoimage. These binding are well documented and do not depend on QT. Using this is the recommended way of interfacing with the image portion of libwkhtmltox.

                Using these binding it is relatively straight forward to convert one or more HTML document to a raster image or SVG document, using the following process:

                wkhtmltoimage_init is called.
                A wkhtmltoimage_global_settings object is creating by calling wkhtmltoimage_create_global_settings.
                Setting for the conversion are set by multiple calls to wkhtmltoimage_set_global_setting.
                A wkhtmltoimage_converter object is created by calling wkhtmltoimage_create_converter, which consumes the global_settings instance.
                A number of callback function are added to the converter object.
                The conversion is performed by calling wkhtmltoimage_convert.
                The converter object is destroyed by calling wkhtmltoimage_destroy_converter.
                */



                byte[] result = new byte[0];
                _imageConverterTools.Init(_options.UseGraphics);


                IntPtr globalSettings = _imageConverterTools.CreateGlobalSettings();
                BindSettings(globalSettings, imageSettings);


                IntPtr converter = string.IsNullOrWhiteSpace(htmlContent) ?
                    _imageConverterTools.CreateConverter(globalSettings) :
                    _imageConverterTools.CreateConverter(globalSettings, htmlContent);
                try
                {

                    _imageConverterTools.SetPhaseChangedCallback(converter, OnPhaseChanged);
                    _imageConverterTools.SetProgressChangedCallback(converter, OnProgressChanged);
                    _imageConverterTools.SetFinishedCallback(converter, OnFinished);
                    _imageConverterTools.SetWarningCallback(converter, OnWarning);
                    _imageConverterTools.SetErrorCallback(converter, OnError);

                    bool isConverted = _imageConverterTools.Convert(converter);
                    if (isConverted)
                    {
                        result = _imageConverterTools.GetResult(converter);
                    }
                    return result;
                }
                finally
                {
                    _imageConverterTools.DestroyConverter(converter);
                }
            }
            finally
            {
                _lock.ReleaseMutex();
            }
        }



        private void OnError(IntPtr converter, string str)
        {
            _logger?.LogError(str);
        }

        private void OnWarning(IntPtr converter, string str)
        {
            _logger?.LogWarning(str);
        }

        private void OnFinished(IntPtr converter, bool val)
        {
            _logger?.LogInformation("Finished");
        }

        private void OnProgressChanged(IntPtr converter, int val)
        {
            _logger?.LogInformation("Changed");
        }

        private void OnPhaseChanged(IntPtr converter)
        {
            _logger?.LogInformation("Phase Changed");
        }

        private void BindSettings(IntPtr globalSetting, object setting)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public;
            var props = setting.GetType().GetProperties(bindingFlags);
            foreach (var prop in props)
            {
                Type propType = prop.GetType();
                var argAttribute = prop.GetCustomAttribute<WkHtmlToImageArgumentAttribute>();

                if (prop.GetValue(setting) == null)
                    continue;

                if (argAttribute != null)
                {
                    if (propType == typeof(bool))
                    {
                        _imageConverterTools.SetGlobalSetting(globalSetting, argAttribute.Argument, ((bool)prop.GetValue(setting)) ? "true" : "false");
                    }
                    else if (propType == typeof(double))
                    {
                        _imageConverterTools.SetGlobalSetting(globalSetting, argAttribute.Argument, ((double)prop.GetValue(setting)).ToString("0.##", CultureInfo.InvariantCulture));
                    }
                    else if (typeof(Dictionary<string, string>).IsAssignableFrom(propType))
                    {
                        var keyValues = (Dictionary<string, string>)prop.GetValue(setting);
                        int index = 0;

                        foreach (var kv in keyValues)
                        {
                            if (kv.Key == null || kv.Value == null)
                            {
                                continue;
                            }
                            _imageConverterTools.SetGlobalSetting(globalSetting, $"{argAttribute.Argument}.append", null);
                            _imageConverterTools.SetGlobalSetting(globalSetting, $"{argAttribute.Argument}[{index}]", $"{kv.Key}\n{kv.Value}");
                            index++;
                        }
                    }
                    else
                    {
                        _imageConverterTools.SetGlobalSetting(globalSetting, argAttribute.Argument, prop.GetValue(setting).ToString());
                    }
                }
                else if (propType == typeof(LoadSettings) || propType == typeof(WebSettings))
                {
                    BindSettings(globalSetting, prop.GetValue(setting));
                }

            }
        }

    }
}
