using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Acme.BookStore
{
    [MyLog]
    public class MyService : ITransientDependency
    {
        private readonly MyOptions _options;
        private readonly IServiceProvider _serviceProvider;
        public MyService(IOptions<MyOptions> options, IServiceProvider serviceProvider)
        {
            _options = options.Value; //Notice the options.Value usage!
            Logger = NullLogger<MyService>.Instance;
            _serviceProvider = serviceProvider;
        }

        public void DoIt()
        {
            var v1 = _options.Value1;
            var v2 = _options.Value2;
        }
        public ILogger<MyService> Logger { get; set; }


        public void DoSomething()
        {
            Logger.LogInformation("123");
            var taxCalculator =(TaxCalculator) _serviceProvider.GetService(typeof(TaxCalculator));
            
            //...使用 Logger 写日志...
        }
    }
}
