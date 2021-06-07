using Microsoft.Extensions.DependencyInjection;
using Structing.Annotations;
using System;

namespace Structing.Single.Sample
{
    [EnableService(ServiceLifetime = ServiceLifetime.Singleton)]
    public class SayHelloService
    {
        public void SayHello()
        {
            Console.WriteLine("Say hello");
        }
    }
}
