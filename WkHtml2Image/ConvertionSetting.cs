namespace WkHtml2Image
{
    public class ConvertionSetting
    {
        /// <summary>
        /// crop.left left/x coordinate of the window to capture in pixels. E.g. "200"
        /// </summary>
        public string CropLeft { get; set; }

        /// <summary>
        /// crop.top top/y coordinate of the window to capture in pixels. E.g. "200"
        /// </summary>
        public string CropTop { get; set; }

        /// <summary>
        /// Width of the window to capture in pixels. E.g. "200"
        /// </summary>
        public string CropWidth { get; set; }

        /// <summary>
        /// Height of the window to capture in pixels. E.g. "200"
        /// </summary>
        public string CropHeight { get; set; }


        /// <summary>
        /// When outputting a PNG or SVG, make the white background transparent. Must be either "true" or "false"
        /// </summary>
        public bool? Transparent { get; set; }


        /// <summary>
        /// The with of the screen used to render is pixels, e.g "800".
        /// </summary>
        public int ScreenWidth { get; set; }

        /// <summary>
        /// The with of the screen used to render is pixels, e.g "800".
        /// </summary>
        public int ScreenHeight { get; set; }

        /// <summary>
        /// Should we expand the screenWidth if the content does not fit? must be either "true" or "false".
        /// </summary>
        public bool? SmartWidth { get; set; }
        
        /// <summary>
        /// The compression factor to use when outputting a JPEG image. E.g. "94".
        /// </summary>
        public int Quality { get; set; }
    }
}
