using System;

namespace MunicipalityTaxes.Core.Logging
{
    public class NullLoggerFactory : ILoggerFactory
    {
        public ILogger Get(Type type)
        {
           return new NullLogger(type);     
        }
    }
}
