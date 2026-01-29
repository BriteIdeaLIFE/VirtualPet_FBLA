namespace VirtualPetApp.Models;

public sealed class PetState
{
    public Pet Pet { get; set; } = new Pet();
    public int TotalSpent { get; set; } = 0;
    public List<Purchase> Purchases { get; set; } = new();
}
