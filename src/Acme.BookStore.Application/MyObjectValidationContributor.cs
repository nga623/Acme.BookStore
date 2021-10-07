using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Validation;

namespace Acme.BookStore
{
    public class MyObjectValidationContributor
     : IObjectValidationContributor, ITransientDependency
    {
        public void AddErrors(ObjectValidationContext context)
        {
            //Get the validating object
           // var obj = context.ValidatingObject;

            //Add the validation errors if available
           // context.Errors.Add(null);
        }
    }

}
