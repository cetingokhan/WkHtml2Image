using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WkHtml2Image.Engine
{
    public unsafe static class WkHtmlToXBindings
    {
        const string DLLNAME = "libwkhtmltox";

        const CharSet CHARSET = CharSet.Unicode;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkhtmltoimage_str_callback(IntPtr converter, [MarshalAs(UnmanagedType.LPStr)] string str);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkhtmltoimage_int_callback(IntPtr converter, int val);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkhtmltoimage_bool_callback(IntPtr converter, bool val);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void wkhtmltoimage_void_callback(IntPtr converter);


        [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkhtmltoimage_extended_qt();

        [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkhtmltoimage_version();

        [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkhtmltoimage_init(int useGraphics);

        [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkhtmltoimage_deinit();

        [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkhtmltoimage_create_global_settings();

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltoimage_set_global_setting(IntPtr settings, IntPtr name, IntPtr value);

        [DllImport(DLLNAME, CharSet = CHARSET)]
        public static extern int wkhtmltoimage_get_global_setting(IntPtr settings, IntPtr name, IntPtr value, int vs);

        [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr wkhtmltoimage_create_converter(IntPtr globalSettings, IntPtr data);

        [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkhtmltoimage_convert(IntPtr converter);

        [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkhtmltoimage_destroy_converter(IntPtr converter);

        [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkhtmltoimage_get_output(IntPtr converter, out IntPtr data);

        [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkhtmltoimage_set_phase_changed_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] wkhtmltoimage_void_callback callback);

        [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkhtmltoimage_set_progress_changed_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] wkhtmltoimage_int_callback callback);

        [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkhtmltoimage_set_finished_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] wkhtmltoimage_bool_callback callback);

        [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkhtmltoimage_set_warning_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] wkhtmltoimage_str_callback callback);

        [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkhtmltoimage_set_error_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] wkhtmltoimage_str_callback callback);

        [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkhtmltoimage_phase_count(IntPtr converter);

        [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkhtmltoimage_current_phase(IntPtr converter);

        [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkhtmltoimage_phase_description(IntPtr converter, int phase);

        [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkhtmltoimage_progress_string(IntPtr converter);

        [DllImport(DLLNAME, CharSet = CHARSET, CallingConvention = CallingConvention.Cdecl)]
        public static extern int wkhtmltoimage_http_error_code(IntPtr converter);

    }
}
