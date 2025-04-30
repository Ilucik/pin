
using Microsoft.Extensions.Options;

namespace pin.Infrastructure
{
    public interface IWritableOptions<T> : IOptions<T> where T : class, new()
    {
        public void Update(Action<T> applyChanges);
    }
}
