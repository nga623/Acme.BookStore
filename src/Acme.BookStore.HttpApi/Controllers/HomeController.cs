using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace Acme.BookStore.Controllers
{
   public class HomeController: AbpController
    {
        public IAsyncResult Get(int i)
        {
            return null;
        }
    }
}
