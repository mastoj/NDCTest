using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NDCTest
{
    public class TestRunner
    {
        public IEnumerable<SpecificationResult> GenereateSpecificationResultForAssemblies(IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(y => this.GenerateSpecificationResult(y));
        }

        public IEnumerable<SpecificationResult> GenerateSpecificationResult(Assembly assembly)
        {
            var specifications = assembly.GetTypes().Where(y => !y.IsAbstract && typeof(IAmSpecification).IsAssignableFrom(y));
            foreach (var specification in specifications)
            {
                IAmSpecification instance = Activator.CreateInstance(specification) as IAmSpecification;
                yield return GenerateSpecificationResult(instance);
            }
        }

        private static SpecificationResult GenerateSpecificationResult(IAmSpecification instance)
        {
            instance.Setup();
            var specificationResult = new SpecificationResult()
            {
                Given = instance.GetGiven().ToString(),
                When = instance.When().ToString(),
                Results = new List<TestResult>()
            };

            var tests =
                instance.GetType().GetMethods().Where(
                    y => y.GetCustomAttribute<TestAttribute>() != null);
            foreach (var test in tests)
            {
                var testResult = new TestResult() { Description = test.Name.Replace("_", " "), Passed = true };
                try
                {
                    test.Invoke(instance, null);
                }
                catch (Exception)
                {
                    testResult.Passed = false;
                }
                specificationResult.Results.Add(testResult);
            }
            return specificationResult;
        }
    }

    public class SpecificationResult
    {
        public string Given { get; set; }
        public string When { get; set; }
        public List<TestResult> Results { get; set; }
    }

    public class TestResult
    {
        public bool Passed { get; set; }
        public string Description { get; set; }
    }
}
