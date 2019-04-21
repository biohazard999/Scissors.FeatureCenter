using Scissors.Xaf.CacheWarmup.Attributes;
using Scissors.Xaf.CacheWarmup.Generators;
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
            var assemblyPath = args[0];
            var mode = Mode.OutOfProcess;
            var foundType = finder.FindAttribute(assemblyPath, mode);

            Console.WriteLine(foundType);

            if(foundType != null)
            {
                var cacheGenerator = new CacheWarmupGenerator();

                var cacheResult = cacheGenerator.WarmupCache(assemblyPath, foundType.ApplicationType, foundType.FactoryType, mode);
                if(cacheResult != null)
                {

                }
            }

            Console.WriteLine("Done");
        }
    }
}
