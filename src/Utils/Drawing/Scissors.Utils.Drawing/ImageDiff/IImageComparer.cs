using System.Drawing;

namespace Scissors.Utils.Drawing.ImageDiff
{
    public interface IImageComparer<T> where T : Image
    {
        T Compare(T firstImage, T secondImage);
        bool Equals(T firstImage, T secondImage);
    }
}