using System;
using System.Drawing;
using Scissors.Utils.Drawing.ImageDiff;
using Scissors.Utils.Testing.XUnit;
using Shouldly;
using Xunit;

namespace Scissors.Utils.Drawing.Tests.ImageDiff
{
    public class BitmapComparerTests
    {
        protected const string TestImage1 = "TestData/ImageDiff/TestImage1.png";
        protected const string TestImage2 = "TestData/ImageDiff/TestImage2.png";
        protected const string OutputFormat = "output_{0}.png";
        protected Bitmap FirstImage { get; set; }
        protected Bitmap SecondImage { get; set; }

        public BitmapComparerTests()
        {
            FirstImage = new Bitmap(this.GetResourceStream(TestImage1));
            SecondImage = new Bitmap(this.GetResourceStream(TestImage2));
        }

        [Fact]
        public void CompareThrowsWhenFirstImageIsNull()
        {

            var target = new BitmapComparer(null);
            Should.Throw<ArgumentNullException>(() => target.Compare(null, SecondImage));
        }

        [Fact]
        public void CompareThrowsWhenSecondImageIsNull()
        {
            var target = new BitmapComparer(null);

            Should.Throw<ArgumentNullException>(() => target.Compare(FirstImage, null));
        }

        [Fact]
        public void CompareThrowsWhenImagesAreNotSameWidth()
        {
            var firstBitmap = new Bitmap(10, 10);
            var secondBitmap = new Bitmap(20, 10);

            var target = new BitmapComparer(null);

            Should.Throw<ArgumentException>(() => target.Compare(firstBitmap, secondBitmap));
        }

        [Fact]
        public void CompareThrowsWhenImagesAreNotSameHeight()
        {
            var firstBitmap = new Bitmap(10, 10);
            var secondBitmap = new Bitmap(10, 20);

            var target = new BitmapComparer(null);

            Should.Throw<ArgumentException>(() => target.Compare(firstBitmap, secondBitmap));
        }

        [Fact]
        public void ImageDiffThrowsWhenBoundingBoxPaddingIsLessThanZero()
        {
            Should.Throw<ArgumentException>(() => new BitmapComparer(new CompareOptions
            {
                BoundingBoxColor = Color.Red,
                BoundingBoxMode = BoundingBoxModes.Single,
                AnalyzerType = AnalyzerTypes.ExactMatch,
                DetectionPadding = 2,
                BoundingBoxPadding = -2,
                Labeler = LabelerTypes.Basic
            }));
        }

        [Fact]
        public void ImageDiffThrowsWhenDetectionPaddingIsLessThanZero()
        {
            Should.Throw<ArgumentException>(() => new BitmapComparer(new CompareOptions
            {
                BoundingBoxColor = Color.Red,
                BoundingBoxMode = BoundingBoxModes.Single,
                AnalyzerType = AnalyzerTypes.ExactMatch,
                DetectionPadding = -2,
                BoundingBoxPadding = 2,
                Labeler = LabelerTypes.Basic
            }));
        }

        [Fact]
        [Integration]
        public void CompareWorksWithNoOptions()
        {
            var target = new BitmapComparer();
            var result = target.Compare(FirstImage, SecondImage);
            result.Save(string.Format(OutputFormat, "CompareWorksWithNullOptions"), SecondImage.RawFormat);
        }

        [Fact]
        [Integration]
        public void CompareWorksWithNullOptions()
        {
            var target = new BitmapComparer(null);
            var result = target.Compare(FirstImage, SecondImage);
            result.Save(string.Format(OutputFormat, "CompareWorksWithNullOptions"), SecondImage.RawFormat);
        }

        [Theory]
        [Integration]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        public void CompareWorksWithIdenticalImages(AnalyzerTypes aType, BoundingBoxModes bMode, LabelerTypes lType)
        {
            var target = new BitmapComparer(new CompareOptions
            {
                AnalyzerType = aType,
                BoundingBoxMode = bMode,
                Labeler = lType
            });
            var result = target.Compare(FirstImage, FirstImage);
            result.Save(string.Format(OutputFormat, $"CompareWorksWithIdenticalImages_{aType}_{bMode}_{lType}"), SecondImage.RawFormat);
        }

        [Theory]
        [Integration]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        public void CompareWorksWithDifferentImages(AnalyzerTypes aType, BoundingBoxModes bMode, LabelerTypes lType)
        {
            var target = new BitmapComparer(new CompareOptions
            {
                BoundingBoxColor = Color.Red,
                BoundingBoxMode = bMode,
                AnalyzerType = aType,
                DetectionPadding = 2,
                BoundingBoxPadding = 2,
                Labeler = lType
            });
            var result = target.Compare(FirstImage, SecondImage);
            result.Save(string.Format(OutputFormat, $"CompareWorksWithDifferentImages_{aType}_{bMode}_{lType}"), SecondImage.RawFormat);
        }

        [Theory]
        [Integration]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        public void EqualsReturnsTrueWithSameImage(AnalyzerTypes aType, BoundingBoxModes bMode, LabelerTypes lType)
        {
            var target = new BitmapComparer(new CompareOptions
            {
                BoundingBoxColor = Color.Red,
                BoundingBoxMode = bMode,
                AnalyzerType = aType,
                DetectionPadding = 2,
                BoundingBoxPadding = 2,
                Labeler = lType
            });

            var newInstanceOfFirstImage = new Bitmap(this.GetResourceStream(TestImage1));
            var result = target.Equals(FirstImage, newInstanceOfFirstImage);
            result.ShouldBeTrue();
        }

        [Theory]
        [Integration]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        public void EqualsReturnsFalseWithDifferentImage(AnalyzerTypes aType, BoundingBoxModes bMode, LabelerTypes lType)
        {
            var target = new BitmapComparer(new CompareOptions
            {
                BoundingBoxColor = Color.Red,
                BoundingBoxMode = bMode,
                AnalyzerType = aType,
                DetectionPadding = 2,
                BoundingBoxPadding = 2,
                Labeler = lType
            });
            var result = target.Equals(FirstImage, SecondImage);
            result.ShouldBeFalse();
        }

        [Theory]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        public void EqualsReturnsTrueWithNullImages(AnalyzerTypes aType, BoundingBoxModes bMode, LabelerTypes lType)
        {
            var target = new BitmapComparer(new CompareOptions
            {
                BoundingBoxColor = Color.Red,
                BoundingBoxMode = bMode,
                AnalyzerType = aType,
                DetectionPadding = 2,
                BoundingBoxPadding = 2,
                Labeler = lType
            });
            var result = target.Equals(null, null);
            result.ShouldBeTrue();
        }

        [Theory]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        public void EqualsReturnsFalseWithNullFirstImage(AnalyzerTypes aType, BoundingBoxModes bMode, LabelerTypes lType)
        {
            var target = new BitmapComparer(new CompareOptions
            {
                BoundingBoxColor = Color.Red,
                BoundingBoxMode = bMode,
                AnalyzerType = aType,
                DetectionPadding = 2,
                BoundingBoxPadding = 2,
                Labeler = lType
            });
            var result = target.Equals(null, SecondImage);
            result.ShouldBeFalse();
        }

        [Theory]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [InlineData(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        public void EqualsReturnsFalseWithNullSecondImage(AnalyzerTypes aType, BoundingBoxModes bMode, LabelerTypes lType)
        {
            var target = new BitmapComparer(new CompareOptions
            {
                BoundingBoxColor = Color.Red,
                BoundingBoxMode = bMode,
                AnalyzerType = aType,
                DetectionPadding = 2,
                BoundingBoxPadding = 2,
                Labeler = lType
            });
            var result = target.Equals(FirstImage, null);
            result.ShouldBeFalse();
        }
    }
}
