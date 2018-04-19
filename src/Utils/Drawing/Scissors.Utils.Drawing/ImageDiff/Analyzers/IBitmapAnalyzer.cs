using System.Drawing;

namespace Scissors.Utils.Drawing.ImageDiff.Analyzers
{
    public interface IBitmapAnalyzer
    {
        bool[,] Analyze(Image first, Image second);
    }
}