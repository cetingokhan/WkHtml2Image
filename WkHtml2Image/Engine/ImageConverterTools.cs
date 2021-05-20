using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WkHtml2Image.Engine
{
    internal sealed class ImageConverterTools : IDisposable
    {
        public bool IsInitialized { get; private set; }

        public ImageConverterTools()
        {
            IsInitialized = false;
        }

        public void Init(bool useGraphics)
        {
            if (IsInitialized)
                return;

            if (WkHtmlToXBindings.wkhtmltoimage_init(useGraphics ? 1 : 0) == 1)
                IsInitialized = true;
        }

        public bool ExtendedQt()
        {
            return WkHtmlToXBindings.wkhtmltoimage_extended_qt() == 1;
        }

        public string GetLibraryVersion()
        {
            return Marshal.PtrToStringAnsi(WkHtmlToXBindings.wkhtmltoimage_version());
        }

        public IntPtr CreateGlobalSettings()
        {
            return WkHtmlToXBindings.wkhtmltoimage_create_global_settings();
        }

        public int SetGlobalSetting(IntPtr settings, string name, string value)
        {
            IntPtr ptrName = Marshal.StringToHGlobalAnsi(name);
            IntPtr ptrValue = Marshal.StringToHGlobalAnsi(value);
            try
            {
                return WkHtmlToXBindings.wkhtmltoimage_set_global_setting(settings, ptrName, ptrValue);
            }
            finally
            {
                Marshal.FreeHGlobal(ptrName);
                Marshal.FreeHGlobal(ptrValue);
            }
        }

        public string GetGlobalSetting(IntPtr settings, string name)
        {
            byte[] buffer = new byte[2048];

            IntPtr tempBuffer = Marshal.AllocHGlobal(buffer.Length);
            IntPtr ptrName = Marshal.StringToHGlobalAnsi(name);
            try
            {
                WkHtmlToXBindings.wkhtmltoimage_get_global_setting(settings, ptrName, tempBuffer, buffer.Length);
                return GetString(buffer);
            }
            finally
            {
                Marshal.FreeHGlobal(tempBuffer);
                Marshal.FreeHGlobal(ptrName);
            }
        }

        public void ClearPointer(IntPtr pointer)
        {
            Marshal.FreeHGlobal(pointer);
        }

        public System.IntPtr ToPointer(string htmlContent)
        {
            if (string.IsNullOrWhiteSpace(htmlContent))
                return System.IntPtr.Zero;

            byte[] strbuffer = Encoding.UTF8.GetBytes(htmlContent);
            System.IntPtr buffer = Marshal.AllocHGlobal(strbuffer.Length + 1);
            Marshal.Copy(strbuffer, 0, buffer, strbuffer.Length);

            //Allocated pointer
            System.IntPtr pointer = new System.IntPtr(buffer.ToInt64() + strbuffer.Length);
            Marshal.WriteByte(pointer, 0);

            return buffer;
        }

        public IntPtr CreateConverter(IntPtr globalSettings)
        {
            return WkHtmlToXBindings.wkhtmltoimage_create_converter(globalSettings, IntPtr.Zero);
        }

        public IntPtr CreateConverter(IntPtr globalSettings, string html)
        {
            return WkHtmlToXBindings.wkhtmltoimage_create_converter(globalSettings, ToPointer(html));
        }

        public bool Convert(IntPtr converter)
        {
            return WkHtmlToXBindings.wkhtmltoimage_convert(converter);
        }

        public void DestroyConverter(IntPtr converter)
        {
            WkHtmlToXBindings.wkhtmltoimage_destroy_converter(converter);
        }

        public byte[] GetResult(IntPtr converter)
        {
            IntPtr resultPointer;

            int length = WkHtmlToXBindings.wkhtmltoimage_get_output(converter, out resultPointer);
            var result = new byte[length];
            Marshal.Copy(resultPointer, result, 0, length);

            return result;
        }

        public int SetPhaseChangedCallback(IntPtr converter, WkHtmlToXBindings.wkhtmltoimage_void_callback callback)
        {
            return WkHtmlToXBindings.wkhtmltoimage_set_phase_changed_callback(converter, callback);
        }

        public int SetProgressChangedCallback(IntPtr converter, WkHtmlToXBindings.wkhtmltoimage_int_callback callback)
        {
            return WkHtmlToXBindings.wkhtmltoimage_set_progress_changed_callback(converter, callback);
        }

        public int SetFinishedCallback(IntPtr converter, WkHtmlToXBindings.wkhtmltoimage_bool_callback callback)
        {
            return WkHtmlToXBindings.wkhtmltoimage_set_finished_callback(converter, callback);
        }

        public int SetWarningCallback(IntPtr converter, WkHtmlToXBindings.wkhtmltoimage_str_callback callback)
        {
            return WkHtmlToXBindings.wkhtmltoimage_set_warning_callback(converter, callback);
        }

        public int SetErrorCallback(IntPtr converter, WkHtmlToXBindings.wkhtmltoimage_str_callback callback)
        {
            return WkHtmlToXBindings.wkhtmltoimage_set_error_callback(converter, callback);
        }

        public int GetPhaseCount(IntPtr converter)
        {
            return WkHtmlToXBindings.wkhtmltoimage_phase_count(converter);
        }

        public int GetCurrentPhase(IntPtr converter)
        {
            return WkHtmlToXBindings.wkhtmltoimage_current_phase(converter);
        }

        public string GetPhaseDescription(IntPtr converter, int phase)
        {
            return Marshal.PtrToStringAnsi(WkHtmlToXBindings.wkhtmltoimage_phase_description(converter, phase));
        }

        public string GetProgressString(IntPtr converter)
        {
            return Marshal.PtrToStringAnsi(WkHtmlToXBindings.wkhtmltoimage_progress_string(converter));
        }


        private bool _disposedValue = false; 

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    //
                }

                WkHtmlToXBindings.wkhtmltoimage_deinit();
                _disposedValue = true;
            }
        }

        ~ImageConverterTools()
        {
            Dispose(false);
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        private string GetString(byte[] buffer)
        {
            //byte[2048]
            int usedIndex = 0;
            while (usedIndex < buffer.Length && buffer[usedIndex] != 0)
            {
                usedIndex++;
            }
            return Encoding.UTF8.GetString(buffer, 0, usedIndex);

        }
    }
}
