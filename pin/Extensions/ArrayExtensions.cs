
namespace pin.Extensions
{
    public static class ArrayExtensions
    {
        public static int IndexOfMin(this int[] src)
        {
            var min = int.MaxValue;
            var ind = -1;
            for (var i = 0; i < src.Length; i++)
            {
                if (src[i] < min)
                {
                    min = src[i];
                    ind = i;
                }
            }

            return ind;
        }
    }
}
