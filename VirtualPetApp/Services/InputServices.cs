using VirtualPet_FBLA.Services_Testing;

namespace VirtualPetApp.Services;

public sealed class InputService
{
    private readonly IConsole _console;

    public InputService(IConsole console) => _console = console;

    public int ReadIntInRange(int min, int max, string? retryPrompt = null)
    {
        while (true)
        {
            string? s = _console.ReadLine();
            if (int.TryParse(s, out int val) && val >= min && val <= max)
                return val;

            _console.Write(retryPrompt ?? $"Please enter a number between {min} and {max}: ");
        }
    }

    public string ReadNonEmptyString(int maxLen, string? retryPrompt = null)
    {
        while (true)
        {
            string s = (_console.ReadLine() ?? "").Trim();

            if (s.Length == 0)
            {
                _console.Write(retryPrompt ?? "Please enter something (not blank): ");
                continue;
            }

            if (s.Length > maxLen)
            {
                _console.Write(retryPrompt ?? $"Please keep it under {maxLen} characters: ");
                continue;
            }

            return s;
        }
    }
}
