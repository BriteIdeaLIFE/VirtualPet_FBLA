namespace VirtualPet_FBLA.Services_Testing;

public interface IFileStore
{
    bool Exists(string path);
    string ReadAllText(string path);
    void WriteAllText(string path, string contents);
    void Delete(string path);
}
