using VirtualPet_FBLA.Services_Testing;

namespace VirtualPetApp.Tests.TestDoubles;

public sealed class InMemoryFileStore : IFileStore
{
    private readonly Dictionary<string, string> _files = new(StringComparer.OrdinalIgnoreCase);

    public bool Exists(string path) => _files.ContainsKey(path);

    public string ReadAllText(string path)
        => _files.TryGetValue(path, out var text) ? text : throw new FileNotFoundException(path);

    public void WriteAllText(string path, string contents) => _files[path] = contents;

    public void Delete(string path) => _files.Remove(path);

    public string? TryGet(string path) => _files.TryGetValue(path, out var v) ? v : null;
}
