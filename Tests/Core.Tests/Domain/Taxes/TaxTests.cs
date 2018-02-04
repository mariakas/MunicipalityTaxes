using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MunicipalityTaxes.Core.Domain.Classifications;
using MunicipalityTaxes.Core.Domain.Taxes;

namespace MunicipalityTaxes.Tests.Core.Tests.Domain.Taxes
{
    [TestClass]
    public class TaxTests
    {
        [TestMethod, TestCategory("Core")]
        public void Tax_Factory_CreateDaily_Works()
        {
            var vilnius = new Municipality() { Id = 1, Name = "Vilnius" };
            var tax = Tax.Factory.CreateDaily(vilnius, Convert.ToDateTime("2018-02-02 12:34"), 0.2M) ;

            tax.MunicipalityId.Should().Be(1);
            tax.TaxType.Should().Be(TaxType.Daily);
            tax.Amount.Should().Be(0.2M);
            tax.From.Should().Be(Convert.ToDateTime("2018-02-02 00:00"));
            tax.To.Should().Be(Convert.ToDateTime("2018-02-02 23:59:59"));
        }
    }
}
