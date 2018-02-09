using Cryptobot.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace Cryptobot.CoinMarketCap
{
    public class CoinMarketClients: IMarket
    {
        private static readonly string BASE_URL = " https://api.coinmarketcap.com/v1/";
        private static readonly HttpClient client = new HttpClient();
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Market>));

        public async Task<IEnumerable<Domain.Market>> AllMarkets(int start = 0, int limit = 100)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var streamTask = client.GetStreamAsync($"{BASE_URL}/ticker?convert=EUR&start={start}&limit={limit}");
            var result = serializer.ReadObject(await streamTask) as List<Market>;

            return result.Select(w => new Domain.Market()
            {
                Name = w.Name,
                Symbol = w.Symbol,
                USDPrice = w.USDPrice,
                EURPrice = w.EURPrice,
                BTCPrice = w.BTCPrice
            });
        }

        public async Task<Domain.Market> Market(String name)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var streamTask = client.GetStreamAsync($"{BASE_URL}/ticker/{name}?convert=EUR");
            var result = serializer.ReadObject(await streamTask) as List<Market>;

            return result.Select(w => new Domain.Market()
            {
                Name = w.Name,
                Symbol = w.Symbol,
                USDPrice = w.USDPrice,
                EURPrice = w.EURPrice,
                BTCPrice = w.BTCPrice
            }).First();
        }
    }

    [DataContract(Name = "market")]
    public class Market
    {
        [DataMember(Name="name")]
        public string Name { get; set; }
        [DataMember(Name = "symbol")]
        public string Symbol { get; set; }
        [DataMember(Name = "price_usd")]
        public string USDPrice { get; set; }
        [DataMember(Name = "price_btc")]
        public string BTCPrice { get; set; }
        [DataMember(Name = "price_eur")]
        public string EURPrice { get; set; }
    }
}
