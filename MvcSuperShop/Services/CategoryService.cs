using MvcSuperShop.Data;

namespace MvcSuperShop.Services;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }
    public CategoriesToReturn GetTrendingCategories(int cnt)
    {
        var categoriesToReturn = new CategoriesToReturn();
        
        if (cnt <= _context.Categories.Count())
        {
            categoriesToReturn.Categories = _context.Categories.Take(cnt);
            categoriesToReturn.status = ReturnStatus.Ok;
            return categoriesToReturn;
        } 
        if (cnt > _context.Categories.Count())
        {
            categoriesToReturn.Categories = _context.Categories.Take(_context.Categories.Count());
            categoriesToReturn.status = ReturnStatus.NotEnoughCategories;
            return categoriesToReturn;
        }
        if (_context.Categories == null)
        {
            categoriesToReturn.Categories = new List<Category>();
            categoriesToReturn.status = ReturnStatus.NoCategoriesExists;
            return categoriesToReturn;
        }
        categoriesToReturn.status = ReturnStatus.SomethingElseWentWrong;
        return categoriesToReturn;
        //return _context.Categories.Take(cnt);
    }
}