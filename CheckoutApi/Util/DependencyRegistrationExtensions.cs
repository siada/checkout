using CheckoutApi.Abstract;
using CheckoutApi.Repository;
using CheckoutApi.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CheckoutApi.Util
{
    public static class DependencyRegistrationExtensions
    {
        public static void RegisterDependencies(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddSingleton(configuration);

            services.AddTransient<IDatabaseService, DatabaseService>();

            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IBasketRepository, BasketRepository>();
        }
    }
}
