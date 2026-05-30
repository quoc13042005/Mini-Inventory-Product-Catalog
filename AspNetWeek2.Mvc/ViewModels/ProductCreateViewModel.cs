using System.ComponentModel.DataAnnotations;

namespace AspNetWeek2.Mvc.ViewModels;

public class ProductCreateViewModel
{
    [Required(ErrorMessage = "Mã SKU không được để trống")]
    public string Sku { get; set; } = "";

    [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
    [StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Nhóm sản phẩm không được để trống")]
    public string Category { get; set; } = "";

    [Required(ErrorMessage = "Nhà cung cấp không được để trống")]
    public string Supplier { get; set; } = "";

    [Range(1000, 100000000, ErrorMessage = "Giá bán phải từ 1.000 đến 100.000.000")]
    public decimal UnitPrice { get; set; }

    [Range(0, 10000, ErrorMessage = "Số lượng phải từ 0 đến 10.000")]
    public int Quantity { get; set; }

    [Range(0, 10000, ErrorMessage = "Mức tồn tối thiểu phải từ 0 đến 10.000")]
    public int MinStock { get; set; }
}
