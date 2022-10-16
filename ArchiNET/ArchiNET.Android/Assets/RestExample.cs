using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace ArchiNET.Droid.Assets
{
    public sealed class WeatherModel
    {
        public short Degrees { get; set; }
        public DateTime FixedAt { get; set; }
        public string Recommendation { get; set; }
    }
    public sealed class RestAPI
    {
        private readonly HttpClient _client;
        public RestAPI()
        {
            _client = new HttpClient() { BaseAddress = new Uri("https://weathersite.com/api/weather/") };
        }
        public async Task<WeatherModel> GetAsync(DateTime dateAt)
        {
            return JsonConvert.DeserializeObject<WeatherModel>(await (await _client.GetAsync($"{dateAt}")).Content.ReadAsStringAsync());
        }
    }
}