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
    public void First_test()
    {
        var guid = Guid.NewGuid();
        var currentCustomer = new CurrentCustomerContext
        {
            UserId = guid,
            Email = "stefan@hejse",
            Agreements = new List<Agreement>()
        };
        Mock<CurrentCustomerContext> cus = new Mock<CurrentCustomerContext>();

    }
}