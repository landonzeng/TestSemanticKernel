using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace AIOT.SemanticKernel.Plugins
{
    public class IPAddressPlugin
    {
        private readonly HttpClient client;
        public IPAddressPlugin()
        {
            this.client = new HttpClient();
        }

        [KernelFunction("GetCityByIP"), Description("返回所在城市")]
        public async Task<IPAddressInfoDTO> GetCityByIP()
        {
            Console.WriteLine("返回所在城市-GetCityByIP");

            string url = $"https://api.ip.sb/geoip/";
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var addressInfo = System.Text.Json.JsonSerializer.Deserialize<IPAddressInfoDTO>(content);
            Console.WriteLine($"\n {addressInfo.ip} 所在地址信息 json:{content}\n");
            return addressInfo;
        }

        #region DTO
        public class IPAddressInfoDTO
        {
            /// <summary>
            /// 
            /// </summary>
            public string organization { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? longitude { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string city { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string timezone { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string isp { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int? offset { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string region { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int? asn { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string asn_organization { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string country { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ip { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double? latitude { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string continent_code { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string country_code { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string region_code { get; set; }
        }
        #endregion
    }
}
