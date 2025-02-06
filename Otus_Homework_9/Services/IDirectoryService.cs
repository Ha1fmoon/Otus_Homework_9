namespace Otus_Homework_9.Services;

public interface IDirectoryService
{
    public event Action<string>? OnErrorThrown;
    bool TryCreateSubDirectoryIfNotExist(DirectoryInfo directory, string name, out DirectoryInfo? subDirectory);
}