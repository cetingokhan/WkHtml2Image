using System;

namespace WkHtml2Image.Engine
{

    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    sealed class WkHtmlToImageArgumentAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly string arg;

        // This is a positional argument
        public WkHtmlToImageArgumentAttribute(string arg)
        {
            this.arg = arg;
           
        }

        public string Argument
        {
            get { return arg; }
        }

    } 
}
