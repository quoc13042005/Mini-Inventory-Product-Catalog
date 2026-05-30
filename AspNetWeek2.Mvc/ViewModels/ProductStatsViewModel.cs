namespace AspNetWeek2.Mvc.ViewModels;

public class ProductStatsViewModel
{
    public int TotalProducts { get; set; }

    public int TotalQuantity { get; set; }

    public decimal TotalInventoryValue { get; set; }

    public int OutOfStockCount { get; set; }

    public int NeedReorderCount { get; set; }

    public string TotalInventoryValueText => $"{TotalInventoryValue:N0} VND";
}