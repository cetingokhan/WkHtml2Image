using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace WkHtml2Image.Tests
{
    public class UnitTest1
    {
        private readonly Mock<ILogger<WkHtml2ImageConverter>> _mockLogger;
        private readonly IOptions<WkHtml2ImageConverterOptions> _options;
        public UnitTest1()
        {
            _mockLogger = new Mock<ILogger<WkHtml2ImageConverter>>();
            _options = Options.Create(new WkHtml2ImageConverterOptions()
            {
                UseGraphics = false
            });
        }

        [Fact]
        public async Task FromHtml()
        {
            WkHtml2ImageConverter converter = new WkHtml2ImageConverter(_options, _mockLogger.Object);

            var sampleContent = @"<html>
                            <head></head>
                            <body>
                                <div style='margin: 0 auto; width: 400px; background-color:yellow; text-align:center; font-size:18pt;'>Hellooooo</div>
                            </body>
                           </html>";
            var result = await converter.Convert(sampleContent, ImageFormat.Png);
            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }

        [Fact]
        public async Task FromUrl()
        {           

            WkHtml2ImageConverter converter = new WkHtml2ImageConverter(_options.Value, _mockLogger.Object);

            var result = await converter.Convert(new Uri("http://www.google.com.tr/"), ImageFormat.Png);
            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }
    }
}
