using VirtualPet_FBLA.Services_Testing;

namespace VirtualPetApp.Tests.TestDoubles;

public sealed class FakeClock : IClock
{
    public DateTime UtcNow { get; set; } = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
}
