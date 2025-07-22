namespace SalesRecords.Shared.SharedKernel.Errors;

public class ApiError
{
    public string Type { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
}