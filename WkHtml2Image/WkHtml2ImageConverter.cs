using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WkHtml2Image.Engine;
using WkHtml2Image.Engine.Settings;

namespace WkHtml2Image
{
    public class WkHtml2ImageConverter : IWkHtml2ImageConverter
    {

        private static ImageConverterTools _imageConverterTools;
        private static WkHtml2ImageConverterOptions _options;
        private readonly ILogger<WkHtml2ImageConverter> _logger;

        public WkHtml2ImageConverter(IOptions<WkHtml2ImageConverterOptions> options, ILogger<WkHtml2ImageConverter> logger)
        {
            _imageConverterTools = new ImageConverterTools();
            _options = options.Value;
            _logger = logger;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var architectureFolder = (IntPtr.Size == 8) ? "64 bit" : "32 bit";

                var paths = Directory.GetFiles(Directory.GetCurrentDirectory(), "libwkhtmltox.dll", SearchOption.AllDirectories).Where(x => x.Contains(architectureFolder)).SingleOrDefault();

                new CustomAssemblyLoadContext().LoadUnmanagedLibrary(paths);
            }
            _logger = logger;
        }

        public async Task<byte[]> Convert(string htmlContent, ImageFormat imageFormat)
        {
            var result = Convert(new ImageSettings()
            {
                Format = imageFormat.ToString().ToLower(),
                //ScreenWidth = 1024,
                //ScreenHeight = 600,
                SmartWidth = true,
                //OutPath = "d:\\google.png",
                LogLevel = 4,
                Quality = 100,
            }, htmlContent);

            return await Task.FromResult(result);
        }

        public async Task<byte[]> Convert(Uri url, ImageFormat imageFormat)
        {

            var result = Convert(new ImageSettings()
            {
                Format = imageFormat.ToString().ToLower(),
                //ScreenWidth = 1024,
                //ScreenHeight = 600,
                SmartWidth = true,
                LogLevel = 4,
                Quality = 100,
                UrlOrInputFile = url.AbsoluteUri
            }, string.Empty);

            return await Task.FromResult(result);
        }


        private byte[] Convert(ImageSettings imageSettings, string htmlContent)
        {
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



        private void OnError(IntPtr converter, string str)
        {
            _logger.LogError(str);
        }

        private void OnWarning(IntPtr converter, string str)
        {
            _logger.LogWarning(str);
        }

        private void OnFinished(IntPtr converter, bool val)
        {
            _logger.LogInformation("Finished");
        }

        private void OnProgressChanged(IntPtr converter, int val)
        {
            _logger.LogInformation("Changed");
        }

        private void OnPhaseChanged(IntPtr converter)
        {
            _logger.LogInformation("Phase Changed");
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
