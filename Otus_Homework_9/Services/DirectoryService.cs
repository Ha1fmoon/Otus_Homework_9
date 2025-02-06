namespace Otus_Homework_9.Services;

public class DirectoryService : IDirectoryService
{
    public event Action<string>? OnErrorThrown;

    public bool TryCreateSubDirectoryIfNotExist
        (DirectoryInfo directory, string name, out DirectoryInfo? subDirectory)
    {
        try
        {
            subDirectory = new DirectoryInfo(Path.Combine(directory.FullName, name));

            if (!subDirectory.Exists) subDirectory = directory.CreateSubdirectory(name);

            return true;
        }
        catch (Exception exception)
        {
            OnErrorThrown?.Invoke($"Cannot get or create subdirectory in {directory.FullName}, {exception.Message}");
            subDirectory = null;
            return false;
        }
    }
}