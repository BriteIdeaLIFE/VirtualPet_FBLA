using FluentAssertions;
using VirtualPetApp.Models;
using VirtualPetApp.Services;
using VirtualPetApp.Tests.TestDoubles;

namespace VirtualPetApp.Tests.Services;

public class GameServiceTests
{
    [Fact]
    public void CreateNewPet_ShouldCreatePetWithChosenTypeAndName()
    {
        // Inputs: name, pet type selection
        var console = new FakeConsole(new string?[] { "Rex", "1" });
        var input = new InputService(console);

        var store = new InMemoryFileStore();
        var persistence = new PersistenceService("save.json", store);

        var clock = new FakeClock();
        var game = new GameService(persistence, input, console, clock);

        // prompts come from CreateNewPet
        console.Write("Pet name: ");
        var state = game.CreateNewPet();

        state.Pet.Name.Should().Be("Rex");
        state.Pet.Type.Should().Be(PetType.Dog);
    }

    [Fact]
    public void MainLoop_WhenUserChoosesSaveAndExit_ShouldPersistAndExit()
    {
        // Sequence:
        // - main menu choice: 8 (Save & Exit)
        // (Loop prints menu; we provide 8 immediately)
        var console = new FakeConsole(new string?[] { "8" });
        var input = new InputService(console);

        var store = new InMemoryFileStore();
        var persistence = new PersistenceService("save.json", store);

        var clock = new FakeClock();
        var game = new GameService(persistence, input, console, clock);

        var state = new PetState { Pet = Pet.CreateDefault(PetType.Cat, "Nori") };

        game.MainLoop(state);

        store.Exists("save.json").Should().BeTrue();
        store.TryGet("save.json").Should().Contain("Nori");
        console.Output.Should().Contain("Saved. Goodbye!");
    }

    [Fact]
    public void MainLoop_ResetYes_ShouldDeleteSaveAndExit()
    {
        // prepare a save
        var store = new InMemoryFileStore();
        store.WriteAllText("save.json", "{\"dummy\":true}");

        // Inputs:
        // - choose 9 reset
        // - confirm y
        var console = new FakeConsole(new string?[] { "9", "y" });
        var input = new InputService(console);
        var persistence = new PersistenceService("save.json", store);
        var clock = new FakeClock();
        var game = new GameService(persistence, input, console, clock);

        var state = new PetState { Pet = Pet.CreateDefault(PetType.Dog, "Bolt") };

        game.MainLoop(state);

        store.Exists("save.json").Should().BeFalse();
        console.Output.Should().Contain("Save deleted");
    }

    [Fact]
    public void MainLoop_ResetNo_ShouldNotDeleteSave()
    {
        var store = new InMemoryFileStore();
        store.WriteAllText("save.json", "{\"dummy\":true}");

        // Inputs:
        // - choose 9 reset
        // - confirm n
        var console = new FakeConsole(new string?[] { "9", "n" });
        var input = new InputService(console);
        var persistence = new PersistenceService("save.json", store);
        var clock = new FakeClock();
        var game = new GameService(persistence, input, console, clock);

        var state = new PetState { Pet = Pet.CreateDefault(PetType.Dog, "Bolt") };

        game.MainLoop(state);

        store.Exists("save.json").Should().BeTrue();
        console.Output.Should().Contain("Reset cancelled");
    }

    [Fact]
    public void FeedAction_ShouldAddPurchase_WithDeterministicTimestamp()
    {
        // This test drives the loop to:
        // - choose Feed (1)
        // - choose Treat (3) costs $2
        // - then Save & Exit (8)
        var clock = new FakeClock { UtcNow = new DateTime(2026, 1, 28, 12, 0, 0, DateTimeKind.Utc) };
        var console = new FakeConsole(new string?[] { "1", "3", "8" });
        var input = new InputService(console);

        var store = new InMemoryFileStore();
        var persistence = new PersistenceService("save.json", store);

        var game = new GameService(persistence, input, console, clock);

        var state = new PetState { Pet = Pet.CreateDefault(PetType.Cat, "Mimi") };

        game.MainLoop(state);

        state.TotalSpent.Should().Be(2);
        state.Purchases.Should().ContainSingle();
        state.Purchases[0].Item.Should().Be("Treat");
        state.Purchases[0].Cost.Should().Be(2);
        state.Purchases[0].WhenUtc.Should().Be(clock.UtcNow);
    }
}
