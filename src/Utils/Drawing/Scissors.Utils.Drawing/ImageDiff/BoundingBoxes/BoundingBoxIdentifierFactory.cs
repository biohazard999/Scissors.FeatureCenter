using System;

namespace Scissors.Utils.Drawing.ImageDiff.BoundingBoxes
{
    /// <summary>
    /// 
    /// </summary>
    public static class BoundingBoxIdentifierFactory
    {
        /// <summary>
        /// Creates the specified mode.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="padding">The padding.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Unrecognized Bounding Box Mode: {mode}</exception>
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