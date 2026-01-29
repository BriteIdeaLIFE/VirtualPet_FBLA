namespace VirtualPet_FBLA.Services_Testing;

public sealed class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
