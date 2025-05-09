using pin.Infrastructure;
using pin.Infrastructure.Models;

namespace pin.Services
{
    public interface IProviderService
    {
        IAsyncEnumerable<PinImage> GetAllImages(string path);
        Task<IEnumerable<PinImage>> GetImages(string path, Pagination pag);
    }
}