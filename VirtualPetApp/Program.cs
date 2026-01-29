using VirtualPet_FBLA.Services_Testing;
using VirtualPetApp.Services;

namespace VirtualPetApp;

internal static class Program
{
    private const string SaveFileName = "virtual_pet_save.json";

    public static void Main()
    {
        Console.Title = "Virtual Pet (FBLA Sample)";
        Console.WriteLine("=== Virtual Pet ===");

        var console = new SystemConsole();
        var store = new PhysicalFileStore();
        var clock = new SystemClock();

        var persistence = new PersistenceService(SaveFileName, store);
        var input = new InputService(console);
        var game = new GameService(persistence, input, console, clock);

        var state = persistence.Load();
        if (state is null)
        {
            console.WriteLine("No saved pet found. Let's create one!\n");
            state = game.CreateNewPet();
            persistence.Save(state);
            console.WriteLine("\nSaved! Starting game...\n");
        }
        else
        {
            console.WriteLine($"Loaded saved pet: {state.Pet.Name} the {state.Pet.Type}\n");
        }

        game.MainLoop(state);
    }
}
