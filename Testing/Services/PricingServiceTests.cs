using Moq;
using MvcSuperShop.Data;
using MvcSuperShop.Infrastructure.Context;
using MvcSuperShop.Services;

namespace Testing.Services;

[TestClass]
public class PricingServiceTests
{
    private PricingService _sut;
    public PricingServiceTests()
    {
        _sut = new PricingService();
    }

    [TestMethod]
    public void When_no_agreement_exists_on_customer_use_baseprice()
    {
        var currentCustomer = new CurrentCustomerContext
        {
            Agreements = new List<Agreement>()
        };

        var products = new List<ProductServiceModel>()
        {
            new ProductServiceModel()
            {
                BasePrice = 1337
            }
        };

        var result = _sut.CalculatePrices(products, currentCustomer);

        Assert.AreEqual(1337, result.First().Price);
    }

    [TestMethod]
    public void When_an_agreement_exists_use_agreement_discount()
    {
        var currentCustomer = new CurrentCustomerContext
        {
            Agreements = new List<Agreement>()
            {
                new Agreement
                {
                    AgreementRows = new List<AgreementRow>()
                    {
                        new AgreementRow
                        {
                            CategoryMatch = "van",
                            PercentageDiscount = 10
                        }
                    }
                }
            }
        };

        var products = new List<ProductServiceModel>()
        {
            new ProductServiceModel()
            {
                BasePrice = 1337,
                CategoryName = "van"
            }
        };

        var result = _sut.CalculatePrices(products, currentCustomer);

        Assert.AreEqual(1203, result.First().Price);
    }

    [TestMethod]
    public void If_multiple_agreements_matches_the_same_product_the_one_with_the_highest_discount_should_be_used()
    {
        var currentCustomer = new CurrentCustomerContext
        {
            Agreements = new List<Agreement>()
            {
                new Agreement
                {
                    AgreementRows = new List<AgreementRow>()
                    {
                        new AgreementRow
                        {
                            CategoryMatch = "van",
                            PercentageDiscount = 3
                        },
                        new AgreementRow
                        {
                            ProductMatch = "Hybrid",
                            PercentageDiscount = 5
                        }
                    }
                },
                new Agreement
                {
                    AgreementRows = new List<AgreementRow>()
                    {
                        new AgreementRow
                        {
                            ManufacturerMatch = "Volvo",
                            PercentageDiscount = 3
                        }
                    }
                }
            }
            
            

        };
        var products = new List<ProductServiceModel>()
        {
            new ProductServiceModel()
            {
                BasePrice = 1000,
                CategoryName = "van",
                Name = "Hybrid",
                
            }
        };

        var result = _sut.CalculatePrices(products, currentCustomer);

        Assert.AreEqual(950, result.First().Price);
    }
}