namespace pin.Services
{
    public class ProviderService
    {
        public async Task LoadImages(string path)
        {
            var source = new List<string>();
            var files = Directory.GetFiles(path);
            
            //var files = Directory.GetFiles(@"C:/rnd","*.jpg");
            for (var i = 0; i < files.Length; i++)
            {
                Image image = new()
                {
                    Source = ImageSource.FromFile(files[i])
                };

                var img = await File.ReadAllBytesAsync(files[i]);
                var imageSource = Convert.ToBase64String(img);
                source.Add(imageSource);
            }

        }
    }
}
