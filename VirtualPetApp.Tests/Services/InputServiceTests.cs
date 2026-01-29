using FluentAssertions;
using VirtualPetApp.Services;
using VirtualPetApp.Tests.TestDoubles;

namespace VirtualPetApp.Tests.Services;

public class InputServiceTests
{
    [Fact]
    public void ReadNonEmptyString_ShouldRejectBlank_AndAcceptValid()
    {
        var console = new FakeConsole(new string?[] { "", "   ", "ValidName" });
        var input = new InputService(console);

        console.Write("Pet name: ");
        var name = input.ReadNonEmptyString(20);

        name.Should().Be("ValidName");
        console.Output.Should().Contain("Please enter something");
    }

    [Fact]
    public void ReadNonEmptyString_ShouldRejectTooLong_ThenAcceptValid()
    {
        var console = new FakeConsole(new string?[] { new string('a', 25), "Short" });
        var input = new InputService(console);

        var value = input.ReadNonEmptyString(10);

        value.Should().Be("Short");
        console.Output.Should().Contain("Please keep it under");
    }

    [Fact]
    public void ReadIntInRange_ShouldRejectInvalid_AndAcceptValid()
    {
        var console = new FakeConsole(new string?[] { "abc", "0", "5", "2" });
        var input = new InputService(console);

        var choice = input.ReadIntInRange(1, 3);

        choice.Should().Be(2);
        console.Output.Should().Contain("Please enter a number between 1 and 3");
    }
}
