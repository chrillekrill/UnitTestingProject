using MvcSuperShop.Data;

namespace MvcSuperShop.Services;

public interface ICategoryService
{
    CategoriesToReturn GetTrendingCategories(int cnt);
}

public class CategoriesToReturn
{
    public IEnumerable<Category> Categories { get; set; }
    public ReturnStatus status { get; set; }

}

public enum ReturnStatus
{
    Ok,
    NotEnoughCategories,
    SomethingElseWentWrong,
    NoCategoriesExists
}