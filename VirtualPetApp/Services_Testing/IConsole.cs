namespace VirtualPet_FBLA.Services_Testing;

public interface IConsole
{
    void Write(string value);
    void WriteLine(string value);
    string? ReadLine();
}
