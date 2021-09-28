using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Acme.BookStore
{

    /// <summary>
    /// 构造方法注入
    /// </summary>
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(ITaxCalculator))]
    public class TaxCalculator : ITaxCalculator, ITransientDependency
    {
        // public string taxRatio { get; set; }
        public double taxRatio { get; set; }
        public TaxCalculator(double taxRatio)
        {
            this.taxRatio = taxRatio;
        }


    }

    public interface ITaxCalculator
    {
        double taxRatio { get; set; }
    }
}
