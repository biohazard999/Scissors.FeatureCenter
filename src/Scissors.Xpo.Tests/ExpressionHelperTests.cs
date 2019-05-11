using System;
using Xunit;
using Shouldly;
using DevExpress.Data.Filtering;
using Scissors.Data;

namespace Scissors.Xpo.Tests
{
    public class ExpressionHelperExtentionsTests
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

        public class GetObjectTypeOperator
        {
            [Fact]
            public void ShouldBeEqualOperatorType() =>
                ExpressionHelper<ExpressionHelperObj>.Create().GetObjectTypeOperator()
                    .OperatorType.ShouldBe(BinaryOperatorType.Equal);

            public class LeftOperand
            {
                [Fact]
                public void ShouldBeOperandProperty() =>
                   ExpressionHelper<ExpressionHelperObj>.Create().GetObjectTypeOperator()
                       .LeftOperand.ShouldBeOfType<OperandProperty>();

                [Fact]
                public void ShouldBeObjectType() =>
                    ((OperandProperty)ExpressionHelper<ExpressionHelperObj>.Create().GetObjectTypeOperator()
                        .LeftOperand).PropertyName.ShouldBe("ObjectType");

                public class Nesting
                {
                    [Fact]
                    public void ShouldBeOperandProperty() =>
                        ExpressionHelper<ExpressionHelperObj>.Create().GetObjectTypeOperator(p => p.NestedType, GetType())
                            .LeftOperand.ShouldBeOfType<OperandProperty>();

                    [Fact]
                    public void ShouldBeObjectType() =>
                       ((OperandProperty)ExpressionHelper<ExpressionHelperObj>.Create().GetObjectTypeOperator(p => p.NestedType, GetType())
                           .LeftOperand).PropertyName.ShouldBe("NestedType.ObjectType");
                }
            }
        }
    }
}
