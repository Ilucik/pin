
namespace pin.Infrastructure
{
    public class PinImage
    {
        public string Src64 { get; private set; }
        public int SourceWidth { get; private set; }
        public int SourceHeight { get; private set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public int Width { get; set; }


        public PinImage(string src64, int width, int height)
        {
            Src64 = src64;
            SourceWidth = width;
            SourceHeight = height;
        }
    }
}
