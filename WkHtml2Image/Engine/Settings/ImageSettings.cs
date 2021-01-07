namespace WkHtml2Image.Engine.Settings
{
    public class ImageSettings
    {
        /// <summary>
        /// crop.left left/x coordinate of the window to capture in pixels. E.g. "200"
        /// </summary>
        [WkHtmlToImageArgument("crop.left")]
        public string CropLeft { get; set; }

        /// <summary>
        /// crop.top top/y coordinate of the window to capture in pixels. E.g. "200"
        /// </summary>
        [WkHtmlToImageArgument("crop.top")]
        public string CropTop { get; set; }

        /// <summary>
        /// Width of the window to capture in pixels. E.g. "200"
        /// </summary>
        [WkHtmlToImageArgument("crop.width")]
        public string CropWidth { get; set; }

        /// <summary>
        /// Height of the window to capture in pixels. E.g. "200"
        /// </summary>
        [WkHtmlToImageArgument("crop.height")]
        public string CropHeight { get; set; }


        /// <summary>
        /// When outputting a PNG or SVG, make the white background transparent. Must be either "true" or "false"
        /// </summary>
        [WkHtmlToImageArgument("transparent")]
        public bool? Transparent { get; set; }

        /// <summary>
        /// The URL or path of the input file, if "-" stdin is used. E.g. "http://google.com"
        /// </summary>
        [WkHtmlToImageArgument("in")]
        public string UrlOrInputFile { get; set; }

        /// <summary>
        /// The path of the output file, if "-" stdout is used, if empty the content is stored to a internalBuffer.
        /// </summary>
        [WkHtmlToImageArgument("out")]
        public string OutPath { get; set; }

        /// <summary>
        /// The output format to use, must be either "", "jpg", "png", "bmp" or "svg".
        /// </summary>
        [WkHtmlToImageArgument("fmt")]
        public string Format { get; set; }

        /// <summary>
        /// The with of the screen used to render is pixels, e.g "800".
        /// </summary>
        [WkHtmlToImageArgument("screenWidth")]
        public int ScreenWidth { get; set; }

        /// <summary>
        /// The with of the screen used to render is pixels, e.g "800".
        /// </summary>
        [WkHtmlToImageArgument("screenHeight")]
        public int ScreenHeight { get; set; }

        /// <summary>
        /// Should we expand the screenWidth if the content does not fit? must be either "true" or "false".
        /// </summary>
        [WkHtmlToImageArgument("smartWidth")]
        public bool? SmartWidth { get; set; }
        
        /// <summary>
        /// The compression factor to use when outputting a JPEG image. E.g. "94".
        /// </summary>
        [WkHtmlToImageArgument("quality")]
        public int Quality { get; set; }

        [WkHtmlToImageArgument("logLevel")]
        public int LogLevel { get; set; }

        public WebSettings WebSettings { get; set; }

        public LoadSettings LoadSettings { get; set; }
    }
}
