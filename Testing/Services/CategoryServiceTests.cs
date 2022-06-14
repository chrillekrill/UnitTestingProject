using Microsoft.EntityFrameworkCore;
using MvcSuperShop.Data;
using MvcSuperShop.Services;

namespace Testing.Services;

[TestClass]
public class CategoryServiceTests
{
    private ApplicationDbContext _context;
    private CategoryService _sut;

    [TestInitialize]
    public void Initialize()
    {
        var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(contextOptions);
        _context.Database.EnsureCreated();

        _sut = new CategoryService(_context);
    }

    [TestMethod]
    public void GetTrendingCategories_should_return_three_categories()
    {
        var categories = new List<Category>
        {
            new Category
            {
                Name = "1",
                Icon = "1",
            },
            new Category
            {
                Name = "2",
                Icon = "2",
            },
            new Category
            {
                Name = "3",
                Icon = "3",
            }
        };

        _context.Categories.AddRange(categories);
        _context.SaveChanges();

        var result = _sut.GetTrendingCategories(3);

        Assert.AreEqual(3, result.Categories.Count());
    }

    [TestMethod]
    public void GetTrendingCategories_should_not_be_able_to_return_more_categories_than_amount_in_database()
    {
        var categories = new List<Category>
        {
            new Category
            {
                Name = "1",
                Icon = "1",
            },
            new Category
            {
                Name = "2",
                Icon = "2",
            },
            new Category
            {
                Name = "3",
                Icon = "3",
            }
        };

        _context.Categories.AddRange(categories);
        _context.SaveChanges();

        var result = _sut.GetTrendingCategories(4);

        Assert.AreEqual(ReturnStatus.NotEnoughCategories, result.status);
        Assert.AreEqual(3, categories.Count);
    }
}