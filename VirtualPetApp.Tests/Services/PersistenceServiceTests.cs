using FluentAssertions;
using VirtualPetApp.Models;
using VirtualPetApp.Services;
using VirtualPetApp.Tests.TestDoubles;

namespace VirtualPetApp.Tests.Services;

public class PersistenceServiceTests
{
    [Fact]
    public void Load_ShouldReturnNull_WhenFileDoesNotExist()
    {
        var store = new InMemoryFileStore();
        var sut = new PersistenceService("save.json", store);

        sut.Load().Should().BeNull();
    }

    [Fact]
    public void SaveThenLoad_ShouldRoundTripState()
    {
        var store = new InMemoryFileStore();
        var sut = new PersistenceService("save.json", store);

        var state = new PetState
        {
            Pet = Pet.CreateDefault(PetType.Cat, "Mochi"),
            TotalSpent = 42
        };

        sut.Save(state);
        var loaded = sut.Load();

        loaded.Should().NotBeNull();
        loaded!.Pet.Name.Should().Be("Mochi");
        loaded.Pet.Type.Should().Be(PetType.Cat);
        loaded.TotalSpent.Should().Be(42);
    }

    [Fact]
    public void Load_ShouldReturnNull_WhenJsonIsCorrupt()
    {
        var store = new InMemoryFileStore();
        store.WriteAllText("save.json", "{not valid json");
        var sut = new PersistenceService("save.json", store);

        sut.Load().Should().BeNull();
    }

    [Fact]
    public void DeleteSaveIfExists_ShouldRemoveFile()
    {
        var store = new InMemoryFileStore();
        store.WriteAllText("save.json", "{\"anything\":true}");
        var sut = new PersistenceService("save.json", store);

        sut.DeleteSaveIfExists();

        store.Exists("save.json").Should().BeFalse();
    }
}
