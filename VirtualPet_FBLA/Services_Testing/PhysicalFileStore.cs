namespace VirtualPet_FBLA.Services_Testing;

public sealed class PhysicalFileStore : IFileStore
{
    public bool Exists(string path) => File.Exists(path);
    public string ReadAllText(string path) => File.ReadAllText(path);
    public void WriteAllText(string path, string contents) => File.WriteAllText(path, contents);
    public void Delete(string path)
    {
        if (File.Exists(path)) File.Delete(path);
    }
}
