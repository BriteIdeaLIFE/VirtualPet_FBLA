namespace VirtualPetApp.Models;

public sealed class Purchase
{
    public string Item { get; set; } = "";
    public int Cost { get; set; }
    public DateTime WhenUtc { get; set; }
}
