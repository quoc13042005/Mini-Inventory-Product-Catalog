namespace AspNetWeek2.Mvc.ViewModels;

public class ProductSearchViewModel
{
    public string Keyword { get; set; } = "";

    public decimal? MinPrice { get; set; }

    public List<ProductListItemViewModel> Products { get; set; } = new();
}
