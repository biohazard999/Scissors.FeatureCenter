using System;
using Scissors.Utils.Drawing.ImageDiff.Labelers;

namespace Scissors.Utils.Drawing.ImageDiff.Labelers
{
    /// <summary>
    /// 
    /// </summary>
    public static class LabelerFactory
    {
        /// <summary>
        /// Creates the specified types.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <param name="padding">The padding.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Unrecognized Analyzer Type: {types}</exception>
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