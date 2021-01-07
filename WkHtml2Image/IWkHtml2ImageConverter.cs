using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WkHtml2Image
{
    public interface IWkHtml2ImageConverter
    {
        Task<byte[]> Convert(Uri url, ImageFormat imageFormat);
        Task<byte[]> Convert(string htmlContent, ImageFormat imageFormat);
    }
}
