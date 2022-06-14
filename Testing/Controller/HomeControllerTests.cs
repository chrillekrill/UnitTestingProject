using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using MvcSuperShop.Controllers;
using MvcSuperShop.Data;
using MvcSuperShop.Infrastructure.Context;
using MvcSuperShop.Services;
using MvcSuperShop.ViewModels;

namespace Testing.Controller;

[TestClass]
public class HomeControllerTests
{
    private HomeController _sut;
    private Mock<ICategoryService> _categoryServiceMock;
    private Mock<IProductService> _productServiceMock;
    private Mock<IMapper> _mapperMock;
    private ApplicationDbContext _context;
    [TestInitialize]
    public void Initialize()
    {
        _categoryServiceMock = new Mock<ICategoryService>();
        _productServiceMock = new Mock<IProductService>();
        _mapperMock = new Mock<IMapper>();

        var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(contextOptions);
        _context.Database.EnsureCreated();

        _sut = new HomeController(_categoryServiceMock.Object, _productServiceMock.Object, _mapperMock.Object, _context);
    }

    [TestMethod]
    public void Index_should_show_three_categories()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Email, "Chrille@chrillescompany.com")
        }, "TestAuthentication"));

        _sut.ControllerContext = new ControllerContext();
        _sut.ControllerContext.HttpContext = new DefaultHttpContext()
        {
            User = user
        };

        //_categoryServiceMock.Setup(e => e.GetTrendingCategories(3)).Returns(new List<Category>
        //{
        //    new Category(),
        //    new Category(),
        //    new Category()
        //});

        _categoryServiceMock.Setup(e => e.GetTrendingCategories(3)).Returns(new CategoriesToReturn
        {
            Categories = new List<Category>
            {
                new Category(),
                new Category(),
                new Category()
            }
        });

        _mapperMock.Setup(m => m.Map<List<CategoryViewModel>>(It.IsAny<List<Category>>()))
            .Returns(new List<CategoryViewModel>
            {
                new CategoryViewModel(),
                new CategoryViewModel(),
                new CategoryViewModel()
            });

        var result = _sut.Index() as ViewResult;

        var model = result.Model as HomeIndexViewModel;

        Assert.AreEqual(3, model.TrendingCategories.Count);
    }

    [TestMethod]
    public void Index_should_return_correct_view()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Email, "Chrille@chrillescompany.com")
        }, "TestAuthentication"));

        _sut.ControllerContext = new ControllerContext();
        _sut.ControllerContext.HttpContext = new DefaultHttpContext()
        {
            User = user
        };

        _categoryServiceMock.Setup(e => e.GetTrendingCategories(It.IsAny<int>())).Returns(new CategoriesToReturn
        {
            Categories = new List<Category>
            {
                new Category(),
                new Category(),
                new Category()
            }
        });

        _mapperMock.Setup(m => m.Map<List<CategoryViewModel>>(It.IsAny<List<Category>>()))
            .Returns(new List<CategoryViewModel>
            {
                new CategoryViewModel(),
                new CategoryViewModel(),
                new CategoryViewModel()
            });

        var result = _sut.Index() as ViewResult;

        Assert.IsNull(result.ViewName);
    }

    

    [TestMethod]
    public void Index_should_set_new_10_products()
    {
        // ARRANGE
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, "gunnar@somecompany.com")
            //other required and custom claims
        }, "TestAuthentication"));

        _sut.ControllerContext = new ControllerContext();
        _sut.ControllerContext.HttpContext = new DefaultHttpContext()
        {
            User = user
        };

        var customer = new CurrentCustomerContext
        {
            UserId = Guid.NewGuid(),
            Email = "blablabla@gmail.com"
        };

        _categoryServiceMock.Setup(e => e.GetTrendingCategories(It.IsAny<int>())).Returns(new CategoriesToReturn
        {
            Categories = new List<Category>
            {
                new Category(),
                new Category(),
                new Category()
            }
        });

        _productServiceMock.Setup(p => p.GetNewProducts(10, customer)).Returns(new List<ProductServiceModel>
            {
                new ProductServiceModel(),
                new ProductServiceModel(),
                new ProductServiceModel(),
                new ProductServiceModel(),
                new ProductServiceModel(),
                new ProductServiceModel(),
                new ProductServiceModel(),
                new ProductServiceModel(),
                new ProductServiceModel(),
                new ProductServiceModel()
            });

        _mapperMock.Setup(m => m.Map<List<ProductBoxViewModel>>(It.IsAny<IEnumerable<ProductServiceModel>>()))
        .Returns(new List<ProductBoxViewModel>
        {
               new ProductBoxViewModel(),
               new ProductBoxViewModel(),
               new ProductBoxViewModel(),
               new ProductBoxViewModel(),
               new ProductBoxViewModel(),
               new ProductBoxViewModel(),
               new ProductBoxViewModel(),
               new ProductBoxViewModel(),
               new ProductBoxViewModel(),
               new ProductBoxViewModel()
        });

        // ACT
        var result = _sut.Index() as ViewResult;

        var model = result.Model as HomeIndexViewModel;

        Assert.AreEqual(10, model.NewProducts.Count);
    }

}