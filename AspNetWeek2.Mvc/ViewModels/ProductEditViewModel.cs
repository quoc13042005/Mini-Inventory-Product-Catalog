namespace AspNetWeek2.Mvc.ViewModels;

public class ProductEditViewModel : ProductCreateViewModel
{
    public int Id { get; set; }
    public string? RowVersion { get; set; }
}
