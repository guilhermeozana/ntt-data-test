namespace DeveloperStore.Shared.SharedKernel.Responses;

public class PagedResponse<T>
{
    public PagedResponse()
    {
    }

    public PagedResponse(IEnumerable<T> data, int totalItems, int currentPage, int totalPages)
    {
        Data = data;
        TotalItems = totalItems;
        CurrentPage = currentPage;
        TotalPages = totalPages;
    }

    public IEnumerable<T> Data { get; set; }
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}