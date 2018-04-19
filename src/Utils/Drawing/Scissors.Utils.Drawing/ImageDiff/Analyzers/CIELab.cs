using System;
using System.Drawing;

namespace Scissors.Utils.Drawing.ImageDiff.Analyzers
{
    internal struct CIELab
    {
        public double L { get; set; }
        public double A { get; set; }
        public double B { get; set; }

        public CIELab(double l, double a, double b)
            : this()
        {
            L = l;
            A = a;
            B = b;
        }

        public static CIELab FromRGB(Color color)
            => FromCIExyz(CIExyz.FromRGB(color));

        public static CIELab FromCIExyz(CIExyz xyzColor)
        {
            var transformedX = Transformxyz(xyzColor.X/ CIExyz.RefX);
            var transformedY = Transformxyz(xyzColor.Y/ CIExyz.RefY);
            var transformedZ = Transformxyz(xyzColor.Z/ CIExyz.RefZ);
            
            var L = 116.0 * transformedY - 16;
            var a = 500.0 * (transformedX - transformedY);
            var b = 200.0 * (transformedY - transformedZ);

            return new CIELab(L, a, b);
        }

        private static double Transformxyz(double t)
            => ((t > 0.008856) ? Math.Pow(t, (1.0 / 3.0)) : ((7.787 * t) + (16.0 / 116.0)));
    }
}