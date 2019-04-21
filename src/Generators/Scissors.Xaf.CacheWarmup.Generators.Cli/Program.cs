using Scissors.Xaf.CacheWarmup.Attributes;
using Scissors.Xaf.CacheWarmup.Generators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Scissors.Xaf.CacheWarmup.Generators.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var assemblyPath = args[0];
            Console.WriteLine($"ARGUMENTS: '{assemblyPath}'");
            var assemblyDirectory = Path.GetDirectoryName(assemblyPath);
            Console.WriteLine($"AssemblyDirectory: '{assemblyDirectory}'");

            AppDomain.CurrentDomain.AssemblyResolve += (object sender, ResolveEventArgs e) =>
            {
                Console.WriteLine($"AssemblyResolve: {e.Name}");
                var assemblyName = e.Name.Split(',').First();
                var aPath = Path.Combine(assemblyDirectory, assemblyName + ".dll");
                Console.WriteLine($"New AssemblyPath: '{aPath}'");
                if (File.Exists(aPath))
                {
                    Console.WriteLine($"New AssemblyPath exists!: '{aPath}'");
                    var loadedAssembly = Assembly.LoadFile(aPath);
                    Console.WriteLine($"New Assembly?: '{loadedAssembly}'");
                    return loadedAssembly;
                }

                return null;
            };

            var finder = new AttributeFinder();
            Console.WriteLine($"Loading Assembly: {assemblyPath}");
            var assembly = Assembly.LoadFile(assemblyPath);
            Console.WriteLine($"Loaded Assembly: {assembly.GetName().FullName}");
            Console.WriteLine($"Try To Find Type");
            var foundType = finder.FindAttribute(assembly);
            Console.WriteLine($"Found-Type: '{foundType}'");

            if (foundType != null)
            {
                var cacheGenerator = new CacheWarmupGenerator();

                var cacheResult = cacheGenerator.WarmupCache(assembly, foundType.ApplicationType, foundType.FactoryType);
                if (cacheResult != null)
                {
                    CopyFile(cacheResult.DcAssemblyFilePath, assemblyDirectory);
                    CopyFile(cacheResult.ModelAssemblyFilePath, assemblyDirectory);
                    CopyFile(cacheResult.ModelCacheFilePath, assemblyDirectory);
                    CopyFile(cacheResult.ModulesVersionInfoFilePath, assemblyDirectory);
                }
            }

            Console.WriteLine("Done");
        }
        private static void CopyFile(string sourceFile, string destFolder)
        {
            Console.WriteLine($"Copy from {sourceFile} to directory: {destFolder}");
            if (File.Exists(sourceFile))
            {
                Console.WriteLine($"Copy from {sourceFile} to directory: {destFolder} exists.");
                var newFile = Path.Combine(destFolder, Path.GetFileName(sourceFile));
                Console.WriteLine($"Should copy from {sourceFile} to destination: {newFile}");
                if (File.Exists(newFile))
                {
                    Console.WriteLine($"Destination exists. Deleting: {newFile}");
                    File.Delete(newFile);
                }
                Console.WriteLine($"Try to copy from {sourceFile} to destination: {newFile}");
                File.Copy(sourceFile, newFile);
                Console.WriteLine($"Copied from {sourceFile} to destination: {newFile}.");
            }
            else
            {
                Console.WriteLine($"Copy from {sourceFile} to directory: {destFolder} does not exist.");
            }
        }
    }
}
