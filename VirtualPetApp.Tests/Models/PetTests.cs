using FluentAssertions;
using VirtualPetApp.Models;

namespace VirtualPetApp.Tests.Models;

public class PetTests
{
    [Theory]
    [InlineData(PetType.Dog)]
    [InlineData(PetType.Cat)]
    [InlineData(PetType.Dragon)]
    [InlineData(PetType.Hamster)]
    public void CreateDefault_ShouldSetTypeAndName_AndClampStats(PetType type)
    {
        var pet = Pet.CreateDefault(type, "Buddy");

        pet.Type.Should().Be(type);
        pet.Name.Should().Be("Buddy");

        pet.Hunger.Should().BeInRange(0, 100);
        pet.Energy.Should().BeInRange(0, 100);
        pet.Cleanliness.Should().BeInRange(0, 100);
        pet.Happiness.Should().BeInRange(0, 100);
        pet.Health.Should().BeInRange(0, 100);

        pet.Mood.Should().BeOneOf(Mood.Happy, Mood.Okay, Mood.Sad, Mood.Sick);
    }

    [Fact]
    public void ClampAll_ShouldClampAllStatsTo0To100()
    {
        var pet = new Pet
        {
            Hunger = -50,
            Energy = 500,
            Cleanliness = -1,
            Happiness = 101,
            Health = 999
        };

        pet.ClampAll();

        pet.Hunger.Should().Be(0);
        pet.Energy.Should().Be(100);
        pet.Cleanliness.Should().Be(0);
        pet.Happiness.Should().Be(100);
        pet.Health.Should().Be(100);
    }

    [Fact]
    public void UpdateMood_ShouldBeSick_WhenHealthBelow30()
    {
        var pet = new Pet
        {
            Health = 29,
            Hunger = 0,
            Energy = 100,
            Cleanliness = 100,
            Happiness = 100
        };

        pet.UpdateMood();
        pet.Mood.Should().Be(Mood.Sick);
    }

    [Fact]
    public void UpdateMood_ShouldBeHappy_WhenOverallScoreHigh()
    {
        var pet = new Pet
        {
            Health = 100,
            Hunger = 0,
            Energy = 100,
            Cleanliness = 100,
            Happiness = 100
        };

        pet.UpdateMood();
        pet.Mood.Should().Be(Mood.Happy);
    }

    [Fact]
    public void UpdateMood_ShouldBeSad_WhenOverallScoreLow()
    {
        var pet = new Pet
        {
            Health = 60,
            Hunger = 100,
            Energy = 0,
            Cleanliness = 0,
            Happiness = 0
        };

        pet.UpdateMood();
        pet.Mood.Should().Be(Mood.Sad);
    }
}
