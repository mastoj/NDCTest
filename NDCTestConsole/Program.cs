using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NDCTest;
using NUnit.Framework;

namespace NDCTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            args = new string[] { @"C:\Users\Tomas Jansson\Documents\Visual Studio 11\Projects\NDCTest\NDCTestTest\bin\Release\NDCTestTest.dll" };
#endif
            var filePaths = args.Select(s => Path.GetFullPath(s)).ToList();
            filePaths.ForEach(Console.WriteLine);
            var assemblies = filePaths.Select(y => Assembly.LoadFrom(y));
            var testRunner = new TestRunner();

            var specificationResult = testRunner.GenereateSpecificationResultForAssemblies(assemblies);
            PrintSpecification(specificationResult);
            Console.ReadLine();
        }

        private static void PrintSpecification(IEnumerable<SpecificationResult> specifiationResults)
        {
            foreach (var specificationResult in specifiationResults)
            {
                Console.WriteLine(specificationResult.Given);
                Console.WriteLine(specificationResult.When);
                foreach (var testResult in specificationResult.Results)
                {
                    Console.WriteLine((testResult.Passed ? "PASS: " : "FAIL: ") + testResult.Description);
                }
            }
        }
    }
}
