using System;
using System.Net;
using System.Web.Http;
using MunicipalityTaxes.Business.Taxes;
using MunicipalityTaxes.Core.Logging;

namespace WebApi.Controllers
{
    public class TaxesController : ApiController
    {
        private static readonly ILogger Log = Logger.Get(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ITaxService TaxService;

        public TaxesController(ITaxService taxService)
        {
            TaxService = taxService;
        }

        // GET api/values
        public decimal Get(string municipality, DateTime date)
        {
            decimal? tax = null; 
            try
            {
                tax = TaxService.Get(municipality, date);
                

                if (tax == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                return tax.Value;
            }
            catch (Exception e)
            {
                Log.Error("Error while geting taxes", e);
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
    }
}
