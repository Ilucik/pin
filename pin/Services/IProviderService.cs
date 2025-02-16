using pin.Infrastructure;

namespace pin.Services
{
    public interface IProviderService
    {
        IAsyncEnumerable<PinImage> GetImages(string path);
    }
}