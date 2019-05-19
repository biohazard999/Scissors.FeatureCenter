using System.Drawing;
using Scissors.Utils.Drawing.ImageDiff;

namespace Scissors.Utils.Drawing.ImageDiff
{
    /// <summary>
    /// 
    /// </summary>
    public class CompareOptions
    {
        /// <summary>
        /// Gets or sets the type of the analyzer.
        /// </summary>
        /// <value>
        /// The type of the analyzer.
        /// </value>
        public AnalyzerTypes AnalyzerType { get; set; }
        /// <summary>
        /// Gets or sets the labeler.
        /// </summary>
        /// <value>
        /// The labeler.
        /// </value>
        public LabelerTypes Labeler { get; set; }
        /// <summary>
        /// Gets or sets the just noticeable difference.
        /// </summary>
        /// <value>
        /// The just noticeable difference.
        /// </value>
        public double JustNoticeableDifference { get; set; }
        /// <summary>
        /// Gets or sets the detection padding.
        /// </summary>
        /// <value>
        /// The detection padding.
        /// </value>
        public int DetectionPadding { get; set; }
        /// <summary>
        /// Gets or sets the bounding box padding.
        /// </summary>
        /// <value>
        /// The bounding box padding.
        /// </value>
        public int BoundingBoxPadding { get; set; }
        /// <summary>
        /// Gets or sets the color of the bounding box.
        /// </summary>
        /// <value>
        /// The color of the bounding box.
        /// </value>
        public Color BoundingBoxColor { get; set; }
        /// <summary>
        /// Gets or sets the bounding box mode.
        /// </summary>
        /// <value>
        /// The bounding box mode.
        /// </value>
        public BoundingBoxModes BoundingBoxMode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompareOptions"/> class.
        /// </summary>
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