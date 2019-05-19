using System.Drawing;

namespace Scissors.Utils.Drawing.ImageDiff
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IImageComparer<T> where T : Image
    {
        /// <summary>
        /// Compares the specified first image.
        /// </summary>
        /// <param name="firstImage">The first image.</param>
        /// <param name="secondImage">The second image.</param>
        /// <returns></returns>
        T Compare(T firstImage, T secondImage);
        /// <summary>
        /// Equals the specified first image.
        /// </summary>
        /// <param name="firstImage">The first image.</param>
        /// <param name="secondImage">The second image.</param>
        /// <returns></returns>
        bool Equals(T firstImage, T secondImage);
    }
}