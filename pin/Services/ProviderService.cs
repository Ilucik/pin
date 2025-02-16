using Microsoft.Maui.Graphics.Platform;
using pin.Infrastructure;

namespace pin.Services
{
    public class ProviderService : IProviderService
    {
        public async IAsyncEnumerable<PinImage> GetImages(string path)
        {
            var files = Directory.GetFiles(path);
            //var files = Directory.GetFiles(path,"*.jpg");
            for (var i = 0; i < files.Length; i++)
            {
                var img = await File.ReadAllBytesAsync(files[i]);
                int height;
                int width;
                using (var ms = new MemoryStream(img))
                {
                    var image = PlatformImage.FromStream(ms);
                    height = Convert.ToInt32(image.Height);
                    width = Convert.ToInt32(image.Width);
                    image.Dispose();
                }

                var imageSource = Convert.ToBase64String(img);
                yield return new PinImage(imageSource, width, height);
            }
        }
    }
}
