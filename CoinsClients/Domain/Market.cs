using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptobot.Domain
{
    public class Market { 
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string USDPrice { get; set; }
        public string BTCPrice { get; set; }
        public string EURPrice { get; set; }

        public String Convert(ManagedConvertion convertion)
        {
            switch(convertion)
            {
                case ManagedConvertion.BTC:
                    return $"{this.BTCPrice} BTC";
                case ManagedConvertion.EUR:
                    return $"{this.EURPrice} €";
                default:
                    return $"{this.BTCPrice} $";
            }
        }
    }

    public enum ManagedConvertion
    {
        USD, EUR, BTC
    }
}
