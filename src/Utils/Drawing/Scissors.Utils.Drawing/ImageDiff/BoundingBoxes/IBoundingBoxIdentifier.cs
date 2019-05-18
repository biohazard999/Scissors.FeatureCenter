using System.Collections.Generic;
using System.Drawing;

namespace Scissors.Utils.Drawing.ImageDiff.BoundingBoxes
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBoundingBoxIdentifier
    {
        /// <summary>
        /// Creates the bounding boxes.
        /// </summary>
        /// <param name="labelMap">The label map.</param>
        /// <returns></returns>
        IEnumerable<Rectangle> CreateBoundingBoxes(int[,] labelMap);
    }
}