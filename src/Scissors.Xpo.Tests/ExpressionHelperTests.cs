using System;
using Xunit;
using Shouldly;
using DevExpress.Data.Filtering;

namespace Scissors.Xpo.Tests
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

        public class GetObjectTypeOperator
        {
            [Fact]
            public void ShouldBeEqualOperatorType() =>
                ExpressionHelper<ExpressionHelperObj>.GetObjectTypeOperator()
                    .OperatorType.ShouldBe(BinaryOperatorType.Equal);

            public class LeftOperand
            {
                [Fact]
                public void ShouldBeOperandProperty() =>
                   ExpressionHelper<ExpressionHelperObj>.GetObjectTypeOperator()
                       .LeftOperand.ShouldBeOfType<OperandProperty>();

                [Fact]
                public void ShouldBeObjectType() =>
                    ((OperandProperty)ExpressionHelper<ExpressionHelperObj>.GetObjectTypeOperator()
                        .LeftOperand).PropertyName.ShouldBe("ObjectType");

                public class Nesting
                {
                    [Fact]
                    public void ShouldBeOperandProperty() =>
                        ExpressionHelper<ExpressionHelperObj>.GetObjectTypeOperator(p => p.NestedType, GetType())
                            .LeftOperand.ShouldBeOfType<OperandProperty>();

                    [Fact]
                    public void ShouldBeObjectType() =>
                       ((OperandProperty)ExpressionHelper<ExpressionHelperObj>.GetObjectTypeOperator(p => p.NestedType, GetType())
                           .LeftOperand).PropertyName.ShouldBe("NestedType.ObjectType");
                }
            }
        }
    }
}
