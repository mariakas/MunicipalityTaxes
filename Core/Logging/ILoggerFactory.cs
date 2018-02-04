using System;

namespace MunicipalityTaxes.Core.Logging
{
    public interface ILoggerFactory
    {
        ILogger Get(Type type);
    }
}
