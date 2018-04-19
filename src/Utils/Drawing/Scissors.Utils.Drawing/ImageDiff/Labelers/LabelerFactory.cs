using System;
using Scissors.Utils.Drawing.ImageDiff.Labelers;

namespace Scissors.Utils.Drawing.ImageDiff.Labelers
{
    public static class LabelerFactory
    {
        public static IDifferenceLabeler Create(LabelerTypes types, int padding)
        {
            switch (types)
            {
                case LabelerTypes.Basic:
                    return new BasicLabeler();
                case LabelerTypes.ConnectedComponentLabeling:
                    return new ConnectedComponentLabeler(padding);
                default:
                    throw new ArgumentException($"Unrecognized Analyzer Type: {types}");
            }
        }
    }
}