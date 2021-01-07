using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WkHtml2Image.Example.WebServer.API
{
    [ApiController]
    [Route("test")]
    public class TestController : ControllerBase
    {
        private readonly IWkHtml2ImageConverter _wkHtml2ImageConverter;

        public TestController(IWkHtml2ImageConverter wkHtml2ImageConverter)
        {
            _wkHtml2ImageConverter = wkHtml2ImageConverter;
        }


        [Route("convert-from-url")]
        [HttpGet]
        public async Task<IActionResult> ConvertFromUrl()
        {
            var result = await _wkHtml2ImageConverter.Convert(new Uri("http://www.google.com/"), ImageFormat.Png);

            return File(result,"image/png","result.png");
        }


        [Route("convert-from-html")]
        [HttpGet]
        public async Task<IActionResult> ConvertFromHtml()
        {
            var sampleContent = @"<html>
                            <head></head>
                            <body>
                                <div style='margin: 0 auto; width: 400px; background-color:yellow; text-align:center; font-size:18pt;'>Hellooooo</div>
                            </body>
                           </html>";

            var result = await _wkHtml2ImageConverter.Convert(sampleContent, ImageFormat.Png);

            return File(result, "image/png", "result.png");
        }
    }
}
