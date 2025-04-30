using Microsoft.Maui.Graphics.Platform;
using pin.Infrastructure;
using pin.Infrastructure.Models;
using System.Text.RegularExpressions;

namespace pin.Services
{
    public class ProviderService : IProviderService
    {
        private readonly string extensionsRegex = $"^.+\\.jpg|jpeg|png|gif|webp$";
        public async IAsyncEnumerable<PinImage> GetImages(string path, Pagination pag)
        {
            var files = Directory
                .EnumerateFiles(path)
                .Where(file => Regex.IsMatch(file, extensionsRegex, RegexOptions.IgnoreCase))
                .Skip(pag.Skip)
                .Take(pag.Take);
            if (!files.Any())
            {
                pag.isEnded = true;
                yield break;
            }

            foreach(var e in await RunWorkersAsync(files, StringToPinImage, 10).ConfigureAwait(false))
            yield return e;
        }

        private async Task<PinImage> StringToPinImage(string str)
        {
            var img = await File.ReadAllBytesAsync(str).ConfigureAwait(false);
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
            return new PinImage(imageSource, width, height);
        }

        public async Task<Tout[]> RunWorkersAsync<Tin, Tout>(IEnumerable<Tin> items, Func<Tin, Task<Tout>> func, int degree)
        {
            List<Task<Tout>> tasks = new();
            using (var source = items.GetEnumerator())
            {
                Task[] jobs = new Task[degree];
                for (int i = 0; i < degree; i++)
                {
                    jobs[i] = ((Func<Task>)(async () =>
                    {
                        while (true)
                        {
                            Task<Tout> task;
                            lock (source)
                            {
                                if (source.MoveNext())
                                {
                                    task = func(source.Current);
                                    tasks.Add(task);
                                }
                                else
                                    break;
                            }
                            await task;
                        }
                    }))();
                }
                await Task.WhenAll(jobs);
            }
            return tasks.Select(t => t.Result).ToArray();
        }

        public async IAsyncEnumerable<PinImage> GetAllImages(string path)
        {
            var files = Directory
                .EnumerateFiles(path)
                .Where(file => Regex.IsMatch(file, extensionsRegex, RegexOptions.IgnoreCase));
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
