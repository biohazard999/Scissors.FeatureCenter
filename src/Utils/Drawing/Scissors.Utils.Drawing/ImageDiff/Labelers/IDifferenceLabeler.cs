using System;

namespace Scissors.Utils.Drawing.ImageDiff.Labelers
{
    public interface IDifferenceLabeler
    {
        int[,] Label(bool[,] differenceMap);
    }
}