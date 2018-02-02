using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace Cryptobot.CoinFalconClient
{
    public class RestClient
    {
        private static readonly string BASE_URL = "https://coinfalcon.com/api/v1/markets";
        private static readonly HttpClient client = new HttpClient();
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MarketResponse));

        public RestClient()
        {

        }

        public async Task<List<MarketResponse.Market>> AllMarkets()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var streamTask = client.GetStreamAsync(BASE_URL);
            var result = serializer.ReadObject(await streamTask) as MarketResponse;

            return result.Markets;
        }
    }

    [DataContract(Name ="markets")]
    public class MarketResponse
    {
        //[JsonProperty("data")]
        [DataMember(Name ="data")]
        public List<Market> Markets { get; set; }

        [DataContract(Name = "market")]
        public class Market
        {
            //[JsonProperty("name")]
            [DataMember(Name = "name")]
            public string Name { get; set; }
            //[JsonProperty("last_price")]
            [DataMember(Name = "last_price")]
            public string LastPrice { get; set; }
        }
    }
}
