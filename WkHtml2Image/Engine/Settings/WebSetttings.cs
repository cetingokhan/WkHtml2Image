namespace WkHtml2Image.Engine.Settings
{
    public class WebSettings
    {
        /// <summary>
        /// Should we print the background? Must be either "true" or "false".
        /// </summary>
        [WkHtmlToImageArgument("web.background")]
        public bool? Background { get; set; }

        /// <summary>
        /// Should we load images? Must be either "true" or "false".
        /// </summary>
        [WkHtmlToImageArgument("web.loadImages")]
        public bool? LoadImages { get; set; }

        /// <summary>
        /// Should we enable javascript? Must be either "true" or "false".
        /// </summary>
        [WkHtmlToImageArgument("web.enableJavascript")]
        public bool? EnableJavascript { get; set; }

        /// <summary>
        /// Should we enable intelligent shrinkng to fit more content on one page? Must be either "true" or "false". Has no effect for wkhtmltoimage.
        /// </summary>
        [WkHtmlToImageArgument("web.enableIntelligentShrinking")]
        public bool? EnableIntelligentShrinking { get; set; }

        /// <summary>
        ///  The minimum font size allowed. E.g. "9"
        /// </summary>
        [WkHtmlToImageArgument("web.minimumFontSize")]
        public int? MinimumFontSize { get; set; }


        /// <summary>
        /// What encoding should we guess content is using if they do not specify it properly? E.g. "utf-8"
        /// </summary>
        [WkHtmlToImageArgument("web.defaultEncoding")]
        public string DefaultEncoding { get; set; }

        /// <summary>
        /// Url er path to a user specified style sheet.
        /// </summary>
        [WkHtmlToImageArgument("web.userStyleSheet")]
        public string UserStyleSheet { get; set; }

        /// <summary>
        /// Should we enable NS plugins, must be either "true" or "false". Enabling this will have limited success.
        /// </summary>
        [WkHtmlToImageArgument("web.enablePlugins")]
        public bool? enablePlugins { get; set; }
    }
}
