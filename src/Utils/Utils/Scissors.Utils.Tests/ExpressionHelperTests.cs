using System;
using System.Linq;
using System.Linq.Expressions;
using Shouldly;
using Xunit;

namespace Scissors.Utils.Tests
{
    public class ExpressionHelperTests
    {
        class TargetClass
        {
            public TargetClass A { get; set; }
            public TargetClass B { get; set; }
            public TargetClass C { get; set; }
        }

        string PropertyName<TRet>(Expression<Func<TargetClass, TRet>> expression)
            => ExpressionHelper.GetPropertyPath(expression);

        [Fact]
        public void SimplePathA()
            => PropertyName(m => m.A).ShouldBe("A");

        [Fact]
        public void SimplePathB()
            => PropertyName(m => m.B).ShouldBe("B");
        
        [Fact]
        public void SimplePathC()
            => PropertyName(m => m.C).ShouldBe("C");

        [Fact]
        public void ComplexPath1()
            => PropertyName(m => m.A.A.A.B.C.A).ShouldBe("A.A.A.B.C.A");
        
        [Fact]
        public void ComplexPath2()
            => PropertyName(m => m.C.A.B).ShouldBe("C.A.B");
    }
}
