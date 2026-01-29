using System;
using System.Text;
using VirtualPet_FBLA.Services_Testing;

namespace VirtualPetApp.Tests.TestDoubles;

public sealed class FakeConsole : IConsole
{
    private readonly Queue<string?> _inputs = new();
    private readonly StringBuilder _output = new();

    public FakeConsole(IEnumerable<string?> inputs)
    {
        foreach (var i in inputs) _inputs.Enqueue(i);
    }

    public string Output => _output.ToString();

    public void Write(string value) => _output.Append(value);
    public void WriteLine(string value) => _output.AppendLine(value);

    public string? ReadLine()
        => _inputs.Count == 0 ? null : _inputs.Dequeue();
}
