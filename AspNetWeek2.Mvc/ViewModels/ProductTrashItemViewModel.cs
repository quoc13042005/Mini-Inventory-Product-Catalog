namespace AspNetWeek2.Mvc.ViewModels;

public class ProductTrashItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime? DeletedAt { get; set; }
}
