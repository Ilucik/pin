
namespace pin.Infrastructure
{
    public class PinImage
    {
        public string Src64;
        public int Width;
        public int Height;
        public int Top;
        public int Left;

        public PinImage(string src64, int width, int height)
        {
            Src64 = src64;
            Width = width;
            Height = height;
        }
    }
}
