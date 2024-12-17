

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;

namespace pin.Infrastructure
{
    public class WritableOptions<T> : IWritableOptions<T> where T : class, new()
    {
        private readonly IOptionsMonitor<T> _options;
        private readonly IConfigurationRoot _configuration;

        public WritableOptions(
            IOptionsMonitor<T> options,
            IConfigurationRoot configuration)
        {
            _options = options;
            _configuration = configuration;
        }

        public T Value => _options.CurrentValue;
        public T Get(string name) => _options.Get(name);

        public void Update(Action<T> applyChanges)
        {
            var fileProvider = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot","appsettings.json");
            var sectionName = typeof(T).Name;

            var jObject = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(fileProvider));
            var sectionObject = jObject.TryGetValue(sectionName, out JToken section) ?
                JsonConvert.DeserializeObject<T>(section.ToString()) : (Value ?? new T());

            applyChanges(sectionObject);

            jObject[sectionName] = JObject.Parse(JsonConvert.SerializeObject(sectionObject));
            File.WriteAllText(fileProvider, JsonConvert.SerializeObject(jObject, Newtonsoft.Json.Formatting.Indented));
            _configuration.Reload();
        }
    }
}
