using System;
using System.Drawing;

namespace Scissors.Utils.Drawing.ImageDiff.Analyzers
{
    public class ExactMatchAnalyzer : IBitmapAnalyzer
    {
        public bool[,] Analyze(Image first, Image second)
        {
            var diff = new bool[first.Width, first.Height];
            for (var x = 0; x < first.Width; x++)
            {
                for (var y = 0; y < first.Height; y++)
                {
                    using (var firstBitmap = new Bitmap(first))
                    using (var secondBitmap = new Bitmap(second))
                    {
                        var firstPixel = firstBitmap.GetPixel(x, y);

                        var secondPixel = secondBitmap.GetPixel(x, y);
                        if (firstPixel != secondPixel)
                        {
                            diff[x, y] = true;
                        }
                    }
                }
            }
            return diff;
        }
    }
}