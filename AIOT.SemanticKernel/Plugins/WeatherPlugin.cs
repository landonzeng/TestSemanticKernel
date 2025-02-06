using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace AIOT.SemanticKernel.Plugins
{
    /// <summary>
    ///一个返回指定城市天气的插件。
    /// </summary>
    public class WeatherPlugin
    {
        private readonly HttpClient client;
        public WeatherPlugin(IServiceProvider serviceProvider)
        {
            this.client = new HttpClient();
        }

        [KernelFunction("GetCityWeather"), Description("获取指定的城市当天的天气。")]
        public async Task<CurrentWeatherDTO> GetCityWeather([Description("指定的城市")] string city)
        {
            try
            {
                Console.WriteLine("获取指定的城市当天的天气-GetCityWeather");

                string url = $"https://api.weatherapi.com/v1/current.json?key=98017b59cac54038a6383152250602&q={city}&aqi=no";
                var response = await client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                var json = System.Text.Json.JsonSerializer.Deserialize<CurrentWeatherDTO>(content);
                Console.WriteLine($"\n{city} 今天的天气 json:{content}\n");
                return json;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ex:{ex.Message}");
                throw;
            }
        }

        [KernelFunction("GetCityWeatherHistoryByDateTime"), Description("获取指定时间城市的历史天气。")]
        public async Task<HistoryWeatherDTO> GetCityWeatherHistoryByDateTime([Description("指定的城市")] string city, [Description("指定的历史时间")] string historyDatetime)
        {
            try
            {
                Console.WriteLine("获取指定时间城市的历史天气-GetCityWeatherHistoryByDateTime");

                string url = $"https://api.weatherapi.com/v1/history.json?key=98017b59cac54038a6383152250602&q={city}&dt={historyDatetime}";
                var response = await client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                var json = System.Text.Json.JsonSerializer.Deserialize<HistoryWeatherDTO>(content);
                Console.WriteLine($"\n{city} {historyDatetime} 的天气 json:{content}\n");
                return json;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ex:{ex.Message}");
                throw;
            }
        }

        [KernelFunction("GetCityWeatherFutureByDateTime"), Description("获取指定时间城市的未来天气。")]
        public async Task<string> GetCityWeatherFutureByDateTime([Description("指定的城市")] string city, [Description("指定的未来时间")] string futureDatetime)
        {
            try
            {
                Console.WriteLine("获取指定时间城市的未来天气-GetCityWeatherFutureByDateTime");

                string url = $"https://api.weatherapi.com/v1/future.json?key=98017b59cac54038a6383152250602&q={city}&dt={futureDatetime}";
                var response = await client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"\n{city} {futureDatetime} 的天气 json:{content}\n");
                return content;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ex:{ex.Message}");
                throw;
            }
        }

        #region DTO
        public class CurrentWeatherDTO
        {
            /// <summary>
            /// 
            /// </summary>
            public CurrentItem current { get; set; }
        }
        public class CurrentItem
        {
            /// <summary>
            /// 
            /// </summary>
            public long? last_updated_epoch { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string last_updated { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? temp_c { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? temp_f { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? wind_mph { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? wind_kph { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? wind_degree { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string wind_dir { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? pressure_mb { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? pressure_in { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? precip_mm { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? precip_in { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? humidity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? cloud { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? feelslike_c { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? feelslike_f { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? windchill_c { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? windchill_f { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? heatindex_c { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? heatindex_f { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? dewpoint_c { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? dewpoint_f { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? vis_km { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? vis_miles { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? uv { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? gust_mph { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? gust_kph { get; set; }
        }
        public class HistoryWeatherDTO
        {
            /// <summary>
            /// 
            /// </summary>
            public double? maxtemp_c { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? maxtemp_f { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? mintemp_c { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? mintemp_f { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? avgtemp_c { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? avgtemp_f { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? maxwind_mph { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? maxwind_kph { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? totalprecip_mm { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? totalprecip_in { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? totalsnow_cm { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? avgvis_km { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? avgvis_miles { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? avghumidity { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? daily_will_it_rain { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? daily_chance_of_rain { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? daily_will_it_snow { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? daily_chance_of_snow { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? uv { get; set; }
        }
        #endregion
    }
}
