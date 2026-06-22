using System.ComponentModel.DataAnnotations;

namespace AspNetWeek2.Mvc.ViewModels;

public class ProductCreateViewModel
{
    [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "SKU là bắt buộc.")]
    [RegularExpression(@"^[A-Z0-9\-]+$", ErrorMessage = "SKU chỉ gồm chữ in hoa, số và dấu -.")]
    public string SKU { get; set; } = string.Empty;

    [Range(1000, 100000000, ErrorMessage = "Giá phải từ 1.000 đến 100.000.000.")]
    public decimal Price { get; set; }

    [Range(0, 100000, ErrorMessage = "Tồn kho phải từ 0 đến 100.000.")]
    public int StockQuantity { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }
}
