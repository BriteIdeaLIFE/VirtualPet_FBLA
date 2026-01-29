using System.Text.Json;
using System.Text.Json.Serialization;
using VirtualPet_FBLA.Services_Testing;
using VirtualPetApp.Models;

namespace VirtualPetApp.Services;

public sealed class PersistenceService
{
    private readonly string _path;
    private readonly IFileStore _store;
    private readonly JsonSerializerOptions _options;

    public PersistenceService(string path, IFileStore store)
    {
        _path = path;
        _store = store;
        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };
    }

    public PetState? Load()
    {
        try
        {
            if (!_store.Exists(_path)) return null;

            var json = _store.ReadAllText(_path);
            var loaded = JsonSerializer.Deserialize<PetState>(json, _options);

            if (loaded?.Pet?.Name is null) return null;
            return loaded;
        }
        catch
        {
            return null;
        }
    }

    public void Save(PetState state)
    {
        var json = JsonSerializer.Serialize(state, _options);
        _store.WriteAllText(_path, json);
    }

    public void DeleteSaveIfExists() => _store.Delete(_path);
}
