using LezzetKapinda.Enums;

namespace LezzetKapinda.Models;

public sealed record Payment
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public DateTimeOffset PaymentDate { get; set; }
}