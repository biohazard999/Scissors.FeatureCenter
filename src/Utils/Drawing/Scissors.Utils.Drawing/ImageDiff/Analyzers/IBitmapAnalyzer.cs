using System.Drawing;

namespace Scissors.Utils.Drawing.ImageDiff.Analyzers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBitmapAnalyzer
    {
        /// <summary>
        /// Analyzes the specified first.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <returns></returns>
        bool[,] Analyze(Image first, Image second);
    }
}