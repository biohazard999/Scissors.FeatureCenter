using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;
using FakeItEasy;
using Scissors.ExpressApp.ModelBuilders;
using Shouldly;
using Xunit;

namespace Scissors.ExpressApp.Tests.ModelBuilders
{
    public class PropertyBuilderTests
    {
        public string StringProperty { get; set; }

        public class EmptyAttribute : Attribute { }

        public class AddAttribute
        {
            [Fact]
            public void ShouldAdd()
            {
                var builder = ModelBuilder.Create<PropertyBuilderTests>(new TypesInfo());
                builder
                    .For(p => p.StringProperty)
                    .WithAttribute<EmptyAttribute>();

                builder.For(p => p.StringProperty)
                    .MemberInfo
                    .FindAttribute<EmptyAttribute>()
                    .ShouldNotBeNull();
            }
        }

        public class RemoveAttribute
        {
            [Fact]
            public void ShouldRemove()
            {
                var builder = ModelBuilder.Create<PropertyBuilderTests>(new TypesInfo());
                builder
                    .For(p => p.StringProperty)
                    .WithAttribute<EmptyAttribute>();

                builder
                    .For(p => p.StringProperty)
                    .RemoveAttribute<EmptyAttribute>();

                builder.For(p => p.StringProperty)
                    .MemberInfo
                    .FindAttribute<EmptyAttribute>()
                    .ShouldBeNull();
            }
        }
    }
}
