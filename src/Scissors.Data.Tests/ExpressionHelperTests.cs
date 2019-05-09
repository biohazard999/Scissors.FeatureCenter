using Shouldly;
using System;
using Xunit;

namespace Scissors.Data.Tests
{
    public class ExpressionHelperTests
    {
        public class ExpressionHelperObj
        {
            public static readonly ExpressionHelper<ExpressionHelperObj> Sut = new ExpressionHelper<ExpressionHelperObj>();

            public string StringProperty { get; set; }
            public NestedClass NestedType { get; set; }

            public class NestedClass
            {
            }
        }

        public class Operand
        {
            [Fact]
            public void ShouldReflectPropertyName() =>
                ExpressionHelperObj.Sut.Operand(m => m.StringProperty)
                    .PropertyName.ShouldBe(nameof(ExpressionHelperObj.StringProperty));
        }

        public class Property
        {
            [Fact]
            public void ShouldReflectPropertyName() =>
                ExpressionHelperObj.Sut.Property(m => m.StringProperty)
                    .ShouldBe(nameof(ExpressionHelperObj.StringProperty));
        }
    }
}
