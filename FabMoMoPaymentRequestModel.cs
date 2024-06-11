namespace Saifr.Console;

public record FabMoMoPaymentRequestModel
{
    public string? CallbackUrl { get; init; }
    public string? Description { get; init; }
    public string? UserGenCode { get; init; }
    public string? Type { get; init; }
    public string? MiscNum { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? PhoneNumber { get; init; }
    public decimal Amount { get; init; }
    public string? Network { get; init; }
    public string? ExtRefNum { get; init; }
}