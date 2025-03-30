using Microsoft.Maui.Graphics.Platform;
using pin.Infrastructure;
using pin.Infrastructure.Models;
using System.Text.RegularExpressions;

namespace pin.Services
{
    public class ProviderService : IProviderService
    {
        public async IAsyncEnumerable<PinImage> GetImages(string path)
        {
            var exensions = new string[] {"jpg","png","gif","webp"};
            var regex = $"^.+\\.{String.Join('|',exensions)}$";
            var files = Directory.EnumerateFiles(path).Where(file => Regex.IsMatch(file, regex,RegexOptions.IgnoreCase));
            foreach (var e in files)
            {
                var img = await File.ReadAllBytesAsync(e);
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

        public async IAsyncEnumerable<PinImage> GetImages(string path, Pagination pag)
        {
            var exensions = new string[] { "jpg", "png", "gif", "webp" };
            var regex = $"^.+\\.{String.Join('|', exensions)}$";
            var files = Directory
                .EnumerateFiles(path)
                .Where(file => Regex.IsMatch(file, regex, RegexOptions.IgnoreCase))
                .Skip(pag.Skip)
                .Take(pag.Take);
            if (!files.Any())
            {
                pag.isEnded = true;
                yield break;
            }
            foreach (var e in files)
            {
                var img = await File.ReadAllBytesAsync(e);
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
