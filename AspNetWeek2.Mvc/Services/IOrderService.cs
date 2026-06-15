using AspNetWeek2.Mvc.ViewModels;

namespace AspNetWeek2.Mvc.Services;

public interface IOrderService
{
    Task CreateOrderAsync(OrderCreateViewModel model);
}
