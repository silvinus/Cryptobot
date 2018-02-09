using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptobot.Interface
{
    public interface IMarket
    {
        Task<IEnumerable<Domain.Market>> AllMarkets(int start = 0, int limit = 100);
        Task<Domain.Market> Market(String name);
    }
}
