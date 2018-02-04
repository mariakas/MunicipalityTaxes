using System;
using MunicipalityTaxes.Consumer.Integrations.MunicipalityTaxes;

namespace MunicipalityTaxes.Consumer.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Get via some container
            IMunicipalityTaxesEndPoint endPoint = new MunicipalityTaxesEndPoint("http://localhost:4044/");
            var municipality = "Vilnius";
            var date = new DateTime(2016, 01, 01);
            var result = endPoint.Get(municipality, date);
            Console.WriteLine($"Result for {municipality} and {date} is {result}");
            Console.ReadLine();
        }
    }
}