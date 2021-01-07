using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace WkHtml2Image
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWkHtmlToImage(this IServiceCollection services, Action<IServiceProvider, WkHtml2ImageConverterOptions> configureOptions)
        {
            services.AddOptions<WkHtml2ImageConverterOptions>()
                .Configure<IServiceProvider>((options, resolver) => configureOptions(resolver, options))
                .PostConfigure(options =>
                {                  
                    //if (!options.UseProxy)
                    //{
                    //    throw new ArgumentNullException();
                    //}
                });
            

            services.TryAddSingleton<IWkHtml2ImageConverter>(resolver => resolver.GetRequiredService<WkHtml2ImageConverter>());
            services.AddSingleton<WkHtml2ImageConverter>();

            return services;
        }

        public static IServiceCollection AddWkHtmlToImage(this IServiceCollection services, Action<WkHtml2ImageConverterOptions> configureOptions)
        {
            return services.AddWkHtmlToImage((_, options) => configureOptions(options));
        }
    }
}
