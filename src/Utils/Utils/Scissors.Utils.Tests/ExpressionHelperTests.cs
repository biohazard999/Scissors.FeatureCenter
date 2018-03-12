using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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

        string Pr<TRet>(Expression<Func<TargetClass, TRet>> expression)
            => ExpressionHelper.GetPropertyPath(expression);

        [Fact]
        public void Test1()
        {
            Pr(m => m.A.A.A.B.C.A).ShouldBe("A.A.A.B.C.A");
            Pr(m => m.C.A.B).ShouldBe("C.A.B");
        }
    }
}
