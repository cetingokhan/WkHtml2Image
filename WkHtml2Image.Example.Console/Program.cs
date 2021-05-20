using System;
using System.Threading.Tasks;

namespace WkHtml2Image.Example.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            WkHtml2ImageConverter wkHtml2ImageConverter = new WkHtml2ImageConverter(new WkHtml2ImageConverterOptions() { UseGraphics = false }, null);


            var sampleContent = @"<html>
                            <head></head>
                            <body>
                                <div style='margin: 0 auto; width: 400px; height:400px; background-color:yellow; text-align:center; font-size:18pt;'>Hellooooo</div>
                            </body>
                           </html>";


            Parallel.For(0, 100, async x =>
            {
                try
                {
                    var result = await wkHtml2ImageConverter.Convert(sampleContent, ImageFormat.Png);
                    System.Console.WriteLine($"{x}->{result.Length}");
                }
                catch(Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            });


           System.Console.ReadLine();
        }
    }
}
