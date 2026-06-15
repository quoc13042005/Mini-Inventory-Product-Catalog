using AspNetWeek2.Mvc.ViewModels;

namespace AspNetWeek2.Mvc.Services;

public interface IProductService
{
    Task<List<ProductListItemViewModel>> GetProductListAsync();
    Task<ProductDetailViewModel?> GetProductDetailAsync(int id);
}
