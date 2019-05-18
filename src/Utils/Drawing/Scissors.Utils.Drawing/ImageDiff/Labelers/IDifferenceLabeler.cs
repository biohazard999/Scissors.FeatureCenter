using System;

namespace Scissors.Utils.Drawing.ImageDiff.Labelers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDifferenceLabeler
    {
        /// <summary>
        /// Labels the specified difference map.
        /// </summary>
        /// <param name="differenceMap">The difference map.</param>
        /// <returns></returns>
        int[,] Label(bool[,] differenceMap);
    }
}