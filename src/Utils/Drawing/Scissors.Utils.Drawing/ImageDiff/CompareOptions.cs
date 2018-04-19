using System.Drawing;
using Scissors.Utils.Drawing.ImageDiff;

namespace Scissors.Utils.Drawing.ImageDiff
{
    public class CompareOptions
    {
        public AnalyzerTypes AnalyzerType { get; set; }
        public LabelerTypes Labeler { get; set; }
        public double JustNoticeableDifference { get; set; }
        public int DetectionPadding { get; set; }
        public int BoundingBoxPadding { get; set; }
        public Color BoundingBoxColor { get; set; }
        public BoundingBoxModes BoundingBoxMode { get; set; }

        public CompareOptions()
        {
            Labeler = LabelerTypes.Basic;
            JustNoticeableDifference = 2.3;
            DetectionPadding = 2;
            BoundingBoxPadding = 2;
            BoundingBoxColor = Color.Red;
            BoundingBoxMode = BoundingBoxModes.Single;
            AnalyzerType = AnalyzerTypes.ExactMatch;
        }
    }
}