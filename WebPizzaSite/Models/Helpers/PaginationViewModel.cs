namespace WebPizzaSite.Models.Helpers;

public class PaginationViewModel
{
    public int TotalItems { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
}