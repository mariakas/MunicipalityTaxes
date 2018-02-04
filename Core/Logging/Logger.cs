using System;

namespace MunicipalityTaxes.Core.Logging
{
    public static class Logger
    {
        private static ILoggerFactory factory;

        public static ILoggerFactory Factory
        {
            get
            {
                if (factory == null)
                {
                    factory = new NullLoggerFactory();
                }
                return factory;
            }
            set { factory = value; }
        }

        public static ILogger For<T>()
        {
            return Logger.Get(typeof(T));
        }

        public static ILogger Get(Type type)
        {
            return Factory.Get(type);
        }
    }
}
