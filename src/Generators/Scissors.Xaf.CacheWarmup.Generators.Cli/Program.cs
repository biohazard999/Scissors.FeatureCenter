using Scissors.Xaf.CacheWarmup.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scissors.Xaf.CacheWarmup.Generators.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var finder = new AttributeFinder();
            var assemblyPath = @"C:\F\github\how-to-precache-an-xaf-winforms-application\src\PreCacheDemo.Win\bin\Debug\PreCacheDemo.Win.exe";
            var foundType = finder.FindAttribute(assemblyPath);

            Console.WriteLine(foundType);

            if(foundType != null)
            {
                var cacheGenerator = new CacheWarmupGenerator();

                var cacheResult = cacheGenerator.WarmupCache(assemblyPath, foundType.ApplicationType, foundType.FactoryType);
                if(cacheResult != null)
                {

                }
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
