using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Auditing;
using Volo.Abp.Modularity;

namespace Acme.BookStore.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddApplication<BookStoreWebModule>();
        }
        

        public void Configure(IApplicationBuilder app)
        {
            app.InitializeApplication();
        }
    }
}
