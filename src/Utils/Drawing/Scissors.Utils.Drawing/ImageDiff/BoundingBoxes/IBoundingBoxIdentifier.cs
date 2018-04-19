using System.Collections.Generic;
using System.Drawing;

namespace Scissors.Utils.Drawing.ImageDiff.BoundingBoxes
{
    public interface IBoundingBoxIdentifier
    {
        IEnumerable<Rectangle> CreateBoundingBoxes(int[,] labelMap);
    }
}