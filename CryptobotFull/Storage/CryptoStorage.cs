using Cryptobot.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptobotFull.Storage
{
    public interface ICryptoStorage
    {
        CryptoData Create(String name);
        void AddCurrency(String name, String currency);
        void SetConvertionPreference(String name, ManagedConvertion convertion);
    }

    public class CryptoStorage : ICryptoStorage
    {
        private readonly Dictionary<String, CryptoData> store = new Dictionary<string, CryptoData>();
        private readonly object lockObject = new object();

        public void AddCurrency(string name, string currency)
        {
            if (store[name] == null)
            {
                throw new Exception("Unknow pseudo...");
            }
            store[name].Currencies.Add(currency);
        }

        public CryptoData Create(string name)
        {
            lock (lockObject)
            {
                if (store[name] == null)
                {
                    store[name] = new CryptoData();
                }
            }
            return store[name];
        }

        public void SetConvertionPreference(string name, ManagedConvertion convertion)
        {
            if (store[name] == null)
            {
                throw new Exception("Unknow pseudo...");
            }
            store[name].Preference = convertion;
        }
    }

    public class CryptoData
    {
        public CryptoData()
        {
            Currencies = new List<string>();
            Preference = ManagedConvertion.EUR;
        }

        public List<String> Currencies { get; set; }
        public ManagedConvertion Preference { get; set; }
    }
}