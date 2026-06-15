namespace AspNetWeek2.Mvc.ViewModels;

public class OrderCreateViewModel
{
    public string CustomerName { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
