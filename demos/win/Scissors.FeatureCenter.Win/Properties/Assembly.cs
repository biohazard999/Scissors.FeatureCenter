using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scissors.FeatureCenter.Win;
using Scissors.Xaf.CacheWarmup.Attributes;

#if RELEASE
[assembly: XafCacheWarmup(typeof(FeatureCenterWindowsFormsApplication), typeof(Program))]
#endif