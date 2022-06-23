using MvcSuperShop.Data;
using MvcSuperShop.Infrastructure.Context;
using MvcSuperShop.Services;

namespace Testing.Services;

[TestClass]
public class PricingServiceTests : BaseTest
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

        var products = new List<ProductServiceModel>
        {
            new ProductServiceModel
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
                    ValidFrom = new DateTime(2020, 05, 10),
                    ValidTo = new DateTime(2024, 05, 10),
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
            Agreements = new List<Agreement>
            {
                new Agreement
                {
                    ValidFrom = new DateTime(2020, 05, 10),
                    ValidTo = new DateTime(2024, 05, 10),
                    AgreementRows = new List<AgreementRow>
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
                    ValidFrom = new DateTime(2020, 05, 10),
                    ValidTo = new DateTime(2024, 05, 10),
                    AgreementRows = new List<AgreementRow>
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
                ManufacturerName = "BMW"

            }
        };

        var result = _sut.CalculatePrices(products, currentCustomer);

        Assert.AreEqual(950, result.First().Price);
    }

    [TestMethod]
    public void When_agreement_validto_is_no_longer_valid_date_use_baseprice()
    {
        var currentCustomer = new CurrentCustomerContext
        {
            Agreements = new List<Agreement>
            {
                new Agreement()
                {
                    ValidFrom = new DateTime(2018, 1, 1),
                    ValidTo = new DateTime(2022, 1, 1),
                    AgreementRows = new List<AgreementRow>
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
                BasePrice = 5000,
                CategoryName = "van",
                Name = "Hybrid",
                ManufacturerName = "Volvo"
            }
        };

        var result = _sut.CalculatePrices(products, currentCustomer);

        Assert.AreEqual(5000, result.First().Price);
    }
}