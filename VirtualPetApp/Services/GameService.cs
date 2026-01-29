using VirtualPet_FBLA.Services_Testing;
using VirtualPetApp.Models;

namespace VirtualPetApp.Services;

public sealed class GameService
{
    private readonly PersistenceService _persistence;
    private readonly InputService _input;
    private readonly IConsole _console;
    private readonly IClock _clock;

    public GameService(PersistenceService persistence, InputService input, IConsole console, IClock clock)
    {
        _persistence = persistence;
        _input = input;
        _console = console;
        _clock = clock;
    }

    public PetState CreateNewPet()
    {
        _console.Write("Pet name: ");
        string name = _input.ReadNonEmptyString(maxLen: 20);

        _console.WriteLine("\nChoose a pet type:");
        foreach (var t in Enum.GetValues<PetType>())
            _console.WriteLine($"  {(int)t}) {t}");

        _console.Write("Selection: ");
        int typeChoice = _input.ReadIntInRange(1, Enum.GetValues<PetType>().Length);
        var type = (PetType)typeChoice;

        var pet = Pet.CreateDefault(type, name);
        pet.UpdateMood();

        return new PetState { Pet = pet };
    }

    public void MainLoop(PetState state)
    {
        while (true)
        {
            ApplyTimeTick(state.Pet);

            _console.WriteLine("");
            PrintStatus(state);
            _console.WriteLine("");
            _console.WriteLine("Choose an action:");
            _console.WriteLine("  1) Feed");
            _console.WriteLine("  2) Play");
            _console.WriteLine("  3) Rest");
            _console.WriteLine("  4) Clean");
            _console.WriteLine("  5) Vet Visit / Health Check");
            _console.WriteLine("  6) View Report");
            _console.WriteLine("  7) Save");
            _console.WriteLine("  8) Save & Exit");
            _console.WriteLine("  9) Reset (Delete save and start over)");
            _console.WriteLine("");
            _console.Write("Selection: ");

            int choice = _input.ReadIntInRange(1, 9);

            switch (choice)
            {
                case 1:
                    DoFeed(state);
                    break;
                case 2:
                    DoPlay(state);
                    break;
                case 3:
                    DoRest(state);
                    break;
                case 4:
                    DoClean(state);
                    break;
                case 5:
                    DoVet(state);
                    break;
                case 6:
                    PrintReport(state);
                    break;
                case 7:
                    _persistence.Save(state);
                    _console.WriteLine("Saved.");
                    break;
                case 8:
                    _persistence.Save(state);
                    _console.WriteLine("Saved. Goodbye!");
                    return;
                case 9:
                    ResetAndExit();
                    return;
            }

            state.Pet.ClampAll();
            state.Pet.UpdateMood();
        }
    }

    private void DoFeed(PetState state)
    {
        var pet = state.Pet;

        _console.WriteLine("\nFeed options:");
        _console.WriteLine("  1) Basic Kibble ($3)");
        _console.WriteLine("  2) Healthy Meal ($6)");
        _console.WriteLine("  3) Treat ($2)");
        _console.Write("Selection: ");
        int choice = _input.ReadIntInRange(1, 3);

        switch (choice)
        {
            case 1:
                AddCost(state, 3, "Basic Kibble");
                pet.Hunger -= 18;
                pet.Happiness += 3;
                break;
            case 2:
                AddCost(state, 6, "Healthy Meal");
                pet.Hunger -= 28;
                pet.Health += 4;
                pet.Happiness += 4;
                break;
            case 3:
                AddCost(state, 2, "Treat");
                pet.Hunger -= 8;
                pet.Happiness += 8;
                break;
        }

        _console.WriteLine($"{pet.Name} eats happily.");
    }

    private void DoPlay(PetState state)
    {
        var pet = state.Pet;

        _console.WriteLine("\nPlay options:");
        _console.WriteLine("  1) Ball/Toy ($5)");
        _console.WriteLine("  2) Walk/Exercise ($0)");
        _console.WriteLine("  3) New Game ($4)");
        _console.Write("Selection: ");
        int choice = _input.ReadIntInRange(1, 3);

        switch (choice)
        {
            case 1:
                AddCost(state, 5, "Ball/Toy");
                pet.Happiness += 14;
                pet.Energy -= 12;
                pet.Cleanliness -= 6;
                break;
            case 2:
                pet.Happiness += 10;
                pet.Energy -= 10;
                pet.Hunger += 6;
                break;
            case 3:
                AddCost(state, 4, "New Game");
                pet.Happiness += 12;
                pet.Energy -= 10;
                break;
        }

        _console.WriteLine($"{pet.Name} plays and looks {pet.Mood.ToString().ToLower()}!");
    }

    private void DoRest(PetState state)
    {
        var pet = state.Pet;
        pet.Energy += 22;
        pet.Hunger += 6;
        pet.Health += 1;

        _console.WriteLine($"{pet.Name} takes a nap and feels more energetic.");
    }

    private void DoClean(PetState state)
    {
        var pet = state.Pet;

        _console.WriteLine("\nClean options:");
        _console.WriteLine("  1) Quick Brush ($2)");
        _console.WriteLine("  2) Full Bath ($6)");
        _console.Write("Selection: ");
        int choice = _input.ReadIntInRange(1, 2);

        switch (choice)
        {
            case 1:
                AddCost(state, 2, "Quick Brush");
                pet.Cleanliness += 14;
                break;
            case 2:
                AddCost(state, 6, "Full Bath");
                pet.Cleanliness += 26;
                pet.Happiness -= 2;
                break;
        }

        _console.WriteLine($"{pet.Name} is cleaner now.");   
    }

    private void DoVet(PetState state)
    {
        var pet = state.Pet;

        AddCost(state, 15, "Vet Visit");
        int boost = pet.Health < 50 ? 18 : 10;

        pet.Health += boost;
        pet.Happiness -= 1;
        pet.Energy -= 2;

        _console.WriteLine($"{pet.Name} gets checked out. Health improved by {boost}.");
    }

    private void PrintReport(PetState state)
    {
        _console.WriteLine("\n=== REPORT ===");
        _console.WriteLine($"Pet: {state.Pet.Name} ({state.Pet.Type})");
        _console.WriteLine($"Mood: {state.Pet.Mood}\n");

        _console.WriteLine("Stats (0-100):");
        _console.WriteLine($"  Hunger:      {state.Pet.Hunger,3}  (lower is better)");
        _console.WriteLine($"  Energy:      {state.Pet.Energy,3}");
        _console.WriteLine($"  Cleanliness: {state.Pet.Cleanliness,3}");
        _console.WriteLine($"  Happiness:   {state.Pet.Happiness,3}");
        _console.WriteLine($"  Health:      {state.Pet.Health,3}\n");

        _console.WriteLine("Cost of Care:");
        _console.WriteLine($"  Total spent: ${state.TotalSpent}");
        _console.WriteLine("  Purchases:");
        if (state.Purchases.Count == 0)
        {
            _console.WriteLine("    (none)");
        }
        else
        {
            foreach (var p in state.Purchases.TakeLast(10))
                _console.WriteLine($"    - ${p.Cost}: {p.Item} ({p.WhenUtc:u})");
            if (state.Purchases.Count > 10)
                _console.WriteLine($"    ...and {state.Purchases.Count - 10} more");
        }

        _console.WriteLine("=== END REPORT ===\n");
    }

    private void PrintStatus(PetState state)
    {
        var pet = state.Pet;
        _console.WriteLine($"[{pet.Name} the {pet.Type}] Mood: {pet.Mood} | Total Cost: ${state.TotalSpent}");
        _console.WriteLine($"Hunger:{Bar(pet.Hunger, invert: true)}");
        _console.WriteLine($"Energy:{Bar(pet.Energy)}");
        _console.WriteLine($"Clean:{Bar(pet.Cleanliness)}");
        _console.WriteLine($"Happy:{Bar(pet.Happiness)}");
        _console.WriteLine($"Health:{Bar(pet.Health)}");
        _console.WriteLine("");
        _console.WriteLine(GetReactionLine(pet));
    }

    private static string GetReactionLine(Pet pet)
    {
        if (pet.Health < 25) return $"{pet.Name} looks sick and needs care soon.";
        if (pet.Hunger > 80) return $"{pet.Name} is very hungry and whines softly.";
        if (pet.Energy < 20) return $"{pet.Name} is exhausted and wants to rest.";
        if (pet.Cleanliness < 25) return $"{pet.Name} is dirty and seems uncomfortable.";
        if (pet.Happiness < 25) return $"{pet.Name} seems sad. Playtime might help.";
        if (pet.Mood == Mood.Happy) return $"{pet.Name} looks delighted and bouncy!";
        return $"{pet.Name} is doing okay.";
    }

    private static string Bar(int value, bool invert = false)
    {
        value = Math.Clamp(value, 0, 100);
        int blocks = (int)Math.Round(value / 10.0);
        blocks = Math.Clamp(blocks, 0, 10);

        if (invert) blocks = 10 - blocks;

        return "[" + new string('#', blocks) + new string('.', 10 - blocks) + $"] {value,3}";
    }

    private static void ApplyTimeTick(Pet pet)
    {
        pet.Hunger += 3;
        pet.Energy -= 2;
        pet.Cleanliness -= 1;

        if (pet.Hunger > 85) pet.Health -= 2;
        if (pet.Energy < 15) pet.Health -= 2;
        if (pet.Cleanliness < 20) pet.Health -= 1;

        if (pet.Hunger > 80) pet.Happiness -= 2;
        if (pet.Energy < 20) pet.Happiness -= 1;
        if (pet.Cleanliness < 25) pet.Happiness -= 1;

        pet.ClampAll();
        pet.UpdateMood();
    }

    private void AddCost(PetState state, int cost, string item)
    {
        state.TotalSpent += cost;
        state.Purchases.Add(new Purchase
        {
            Item = item,
            Cost = cost,
            WhenUtc = _clock.UtcNow
        });
    }

    private void ResetAndExit()
    {
        _console.Write("\nAre you sure you want to delete the save and start over? (y/n): ");
        var answer = (_console.ReadLine() ?? "").Trim().ToLowerInvariant();
        if (answer == "y" || answer == "yes")
        {
            _persistence.DeleteSaveIfExists();
            _console.WriteLine("Save deleted. Restart the app to create a new pet.");
        }
        else
        {
            _console.WriteLine("Reset cancelled.");
        }
    }
}
