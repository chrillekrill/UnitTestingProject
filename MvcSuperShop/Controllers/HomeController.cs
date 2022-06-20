using Microsoft.AspNetCore.Mvc;
using MvcSuperShop.Models;
using System.Diagnostics;
using AutoMapper;
using MvcSuperShop.Data;
using MvcSuperShop.Services;
using MvcSuperShop.ViewModels;

namespace MvcSuperShop.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public HomeController(ICategoryService categoryService, IProductService productService, IMapper mapper, ApplicationDbContext context)
        :base(context)
        {
            _categoryService = categoryService;
            _productService = productService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var currentContext = GetCurrentCustomerContext();
            var trendingCategories = _categoryService.GetTrendingCategories(3).Categories;
            var products = _productService.GetNewProducts(10, currentContext);
            var model = new HomeIndexViewModel
            {
                TrendingCategories = _mapper.Map<List<CategoryViewModel>>(trendingCategories),
                NewProducts = _mapper.Map<List<ProductBoxViewModel>>(products)
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}