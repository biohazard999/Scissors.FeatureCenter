using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Xunit;

#pragma warning disable xUnit1019 // MemberData must reference a member providing a valid data type

namespace Scissors.Utils.Tests
{
    public class ResourceExtentionsTests
    {
        public static IEnumerable<object[]> TestData
        {
            get
            {
                yield return new object[] { "TestResources.TestEmbeddedResource.txt" };
                yield return new object[] { "TestResources\\TestEmbeddedResource.txt" };
                yield return new object[] { "TestResources/TestEmbeddedResource.txt" };
            }
        }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
        [Theory]
        [MemberData(nameof(TestData))]
        public void ExistingResourceIsNotNullWithType(string path)
        {
            using (var resourceStream = typeof(ResourceExtentionsTests).GetResourceStream(path))
            {
                resourceStream.ShouldNotBe(null);
            }
        }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
        [Theory]
        [MemberData(nameof(TestData))]
        public void ExistingResourceIsNotNullWithObject(string path)
        {
            using (var resourceStream = this.GetResourceStream(path))
            {
                resourceStream.ShouldNotBe(null);
            }
        }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
        [Fact]
        public void TypeGuardsAgainstNull()
        {
            Type? t = null;
            Should.Throw<ArgumentNullException>(() => t.GetResourceStream(null));
        }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.
        [Fact]
        public void ObjectGuardsAgainstNull()
        {
            object? obj = null;
            Should.Throw<ArgumentNullException>(() => obj.GetResourceStream(null));
        }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference or unconstrained type parameter.

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void TypeGuardsAgainstInvalidStrings(string invalidString)
            => Should.Throw<ArgumentException>(() => this.GetResourceStream(invalidString));
        
        [Theory]
        [InlineData("Resource.Is.Not.Valid.xml")]
        [InlineData(" ")]
        public void ThrowsResourceNotFoundExceptionWithNotExistingResource(string resourceName)
        {
            var name = GetType().Assembly.GetName().Name;
            
            var resourcePath = name + "." + resourceName;
            var execption = Should.Throw<ResourceNotFoundException>(() => GetType().GetResourceStream(resourceName));

            execption.ShouldSatisfyAllConditions(
                () => execption.ResourcePath.ShouldBe(resourcePath),
                () => execption.Assembly.ShouldBe(GetType().Assembly),
                () => execption.ResourceName.ShouldBe(resourceName)
            );
        }
    }
}
