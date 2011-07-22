using System;
using System.Configuration;

namespace mPower.MembershipApi
{
    public class Config
    {
        private static volatile Config _instance;
        private static readonly object _syncRoot = new Object();

        private Config() { }

        public static Config Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new Config();
                            _instance.Read();
                        }
                    }
                }

                return _instance;
            }
        }

        public string ApiBaseUrl { get; private set; }

        public string ApiPrivateKey { get; private set; }

        public void Read()
        {
            ApiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];
            if (String.IsNullOrEmpty(ApiBaseUrl))
                throw new ArgumentException("ApiBaseUrl was not found in current application AppSettings");

            ApiPrivateKey = ConfigurationManager.AppSettings["ApiPrivateKey"];
            if (String.IsNullOrEmpty(ApiPrivateKey))
                throw new ArgumentException("ApiPrivateKey was not found in current application AppSettings");
        }

    }
}
