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
public class HomeControllerTests : BaseControllerTest
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
        _sut.ControllerContext = SetupControllerContext();

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

        var model = result.Model as HomeIndexViewModel;

        Assert.AreEqual(3, model.TrendingCategories.Count);
    }

    [TestMethod]
    public void Index_should_return_correct_view()
    {

        _sut.ControllerContext = SetupControllerContext();


        //Jag vet att du hatar kommentarer, men det kraschar om jag inte mockar den här skiten eftersom jag ändrade i categoryservice
        //och jag vet inte varför
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
        var viewName = result.ViewName;

        Assert.IsTrue(string.IsNullOrEmpty(viewName) || viewName == "Index");
    }


    [TestMethod]
    public void Index_should_set_new_10_products()
    {
        // ARRANGE
        _sut.ControllerContext = SetupControllerContext();

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