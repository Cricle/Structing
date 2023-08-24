using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structing.AspNetCore.Test
{
    [TestClass]
    public class AspNetCoreServicePickerTest
    {
        internal class Startup
        {

        }
        [TestMethod]
        public void ThePickerMustEqualsEnv()
        {
            var host = new WebApplicationFactory<Startup>();

            host.Services.GetRequiredService<IWebHostEnvironment>();
        }
    }
}
