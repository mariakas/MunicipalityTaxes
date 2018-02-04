using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MunicipalityTaxes.Consumer.Integrations.MunicipalityTaxes
{
    public class MunicipalityTaxesEndPoint : IMunicipalityTaxesEndPoint
    {
        private readonly string address;

        public MunicipalityTaxesEndPoint(string address)
        {
            this.address = address;
        }

        public decimal Get(string municipality, DateTime date)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    using (HttpResponseMessage response = client.GetAsync($"{address}/api/taxes?municipality={municipality}&date={date}").Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return response.Content.ReadAsAsync<decimal>().Result;
                        }
                        else
                        {
                            // TODO: 
                            throw new Exception();
                        }
                    }
                }
            }
            catch 
            {
                // TODO:
                throw;
            }
        }

        public Task<decimal> GetAsync(string municipality, DateTime date)
        {
            // TODO:
            throw new NotImplementedException();
        }
    }
}
