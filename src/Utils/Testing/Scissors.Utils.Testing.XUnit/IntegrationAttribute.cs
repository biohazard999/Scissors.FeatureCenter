using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Scissors.Utils.Testing.XUnit
{
    public class IntegrationAttribute : CategoryAttribute
    {
        public override string Category => "Integration";
    }
}
