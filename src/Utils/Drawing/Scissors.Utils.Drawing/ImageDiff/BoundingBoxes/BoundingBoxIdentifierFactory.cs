using System;

namespace Scissors.Utils.Drawing.ImageDiff.BoundingBoxes
{
    public static class BoundingBoxIdentifierFactory
    {
        public static IBoundingBoxIdentifier Create(BoundingBoxModes mode, int padding)
        {
            switch (mode)
            {
                case BoundingBoxModes.Single:
                    return new SingleBoundingBoxIdentifier(padding);
                case BoundingBoxModes.Multiple:
                    return new MultipleBoundingBoxIdentifier(padding);
                default:
                    throw new ArgumentException($"Unrecognized Bounding Box Mode: {mode}");
            }
        }
    }
}