using System;
using System.Drawing;

namespace Scissors.Utils.Drawing.ImageDiff.Analyzers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Scissors.Utils.Drawing.ImageDiff.Analyzers.IBitmapAnalyzer" />
    public class CIE76Analyzer : IBitmapAnalyzer
    {
        private double JustNoticeableDifference { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CIE76Analyzer"/> class.
        /// </summary>
        /// <param name="justNoticeableDifference">The just noticeable difference.</param>
        public CIE76Analyzer(double justNoticeableDifference)
            => JustNoticeableDifference = justNoticeableDifference;


        /// <summary>
        /// Analyzes the specified first.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <returns></returns>
        public bool[,] Analyze(Image first, Image second)
        {
            var diff = new bool[first.Width, first.Height];

            Bitmap firstBitmap;
            var disposeFirstBitmap = false;
            Bitmap secondBitmap;
            var disposeSecondBitmap = false;

            if(first is Bitmap) //Perf
            {
                firstBitmap = (Bitmap)first;
            }
            else
            {
                firstBitmap = new Bitmap(first);
                disposeFirstBitmap = true;
            }

            if(second is Bitmap) //Perf
            {
                secondBitmap = (Bitmap)second;
            }
            else
            {
                secondBitmap = new Bitmap(second);
                disposeSecondBitmap = true;
            }

            try
            {

                for(var x = 0; x < first.Width; x++)
                {
                    for(var y = 0; y < first.Height; y++)
                    {
                        var firstLab = CIELab.FromRGB(firstBitmap.GetPixel(x, y));
                        var secondLab = CIELab.FromRGB(secondBitmap.GetPixel(x, y));

                        var score = Math.Sqrt(Math.Pow(secondLab.L - firstLab.L, 2) +
                                              Math.Pow(secondLab.A - firstLab.A, 2) +
                                              Math.Pow(secondLab.B - firstLab.B, 2));

                        diff[x, y] = (score >= JustNoticeableDifference);
                    }
                }
            }
            finally
            {
                if(disposeFirstBitmap)
                {
                    firstBitmap.Dispose();
                }

                if(disposeSecondBitmap)
                {
                    secondBitmap.Dispose();
                }
            }

            return diff;
        }
    }
}
