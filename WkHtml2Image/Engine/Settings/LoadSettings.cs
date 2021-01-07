using System.Collections.Generic;

namespace WkHtml2Image.Engine.Settings
{
    public class LoadSettings
    {
        /// <summary>
        /// The user name to use when loging into a website, E.g. "bart"
        /// </summary>
        [WkHtmlToImageArgument("load.username")]
        public string Username { get; set; }

        /// <summary>
        ///  The password to used when logging into a website, E.g. "elbarto"
        /// </summary>
        [WkHtmlToImageArgument("load.password")]
        public string Password { get; set; }

        /// <summary>
        /// The mount of time in milliseconds to wait after a page has done loading until it is actually printed. E.g. "1200". We will wait this amount of time or until, javascript calls window.print().
        /// </summary>
        [WkHtmlToImageArgument("load.jsdelay")]
        public int? JSDelay { get; set; }

        /// <summary>
        /// How much should we zoom in on the content. E.g. "2.2".
        /// </summary>
        [WkHtmlToImageArgument("load.zoomFactor")]
        public double? ZoomFactor { get; set; }

        /// <summary>
        /// Disallow local and piped files to access other local files. Must be either "true" or "false".
        /// </summary>
        [WkHtmlToImageArgument("load.blockLocalFileAccess")]
        public bool? BlockLocalFileAccess { get; set; }

        /// <summary>
        /// Stop slow running javascript. Must be either "true" or "false".
        /// </summary>
        [WkHtmlToImageArgument("load.stopSlowScript")]
        public bool? StopSlowScript { get; set; }

        /// <summary>
        /// FForward javascript warnings and errors to the warning callback. Must be either "true" or "false".
        /// </summary>
        [WkHtmlToImageArgument("load.debugJavascript")]
        public bool? DebugJavascript { get; set; }


        /// <summary>
        ///  String describing what proxy to use when loading the object.
        /// </summary>
        [WkHtmlToImageArgument("load.proxy")]
        public string Proxy { get; set; }

        /// <summary>
        /// Custom headers used when requesting page. Defaulty = empty
        /// </summary>
        [WkHtmlToImageArgument("load.customHeaders")]
        public Dictionary<string, string> CustomHeaders { get; set; }

        /// <summary>
        /// Should the custom headers be sent all elements loaded instead of only the main page. Default = false
        /// </summary>
        [WkHtmlToImageArgument("load.repeatCustomHeaders")]
        public bool? RepeatCustomHeaders { get; set; }

        /// <summary>
        /// Cookies used when requesting page. Default = empty
        /// </summary>
        [WkHtmlToImageArgument("load.cookies")]
        public Dictionary<string, string> Cookies { get; set; }
    }
}
