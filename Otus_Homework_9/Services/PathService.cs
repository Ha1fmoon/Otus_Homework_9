namespace Otus_Homework_9.Services;

public class PathService : IPathService
{
    private readonly IDirectoryService _directoryService;
    public event Action<string>? OnErrorThrown;

    public PathService(IDirectoryService directoryService)
    {
        _directoryService = directoryService;
    }


    public DirectoryInfo CreatePath(string root, string path)
    {
        var rootDirectory = ValidateRootDirectory(root);

        return BuildPath(rootDirectory, path);
    }

    private DirectoryInfo ValidateRootDirectory(string root)
    {
        var rootDirectory = new DirectoryInfo(root);

        if (!rootDirectory.Exists)
            throw new DirectoryNotFoundException($"Root ({root}) directory does not exist.");

        return rootDirectory;
    }

    private DirectoryInfo BuildPath(DirectoryInfo rootDirectory, string path)
    {
        var pathSequence = path.Split('\\');
        var currentDirectory = rootDirectory;

        foreach (var dirName in pathSequence) currentDirectory = CreateSubDirectory(currentDirectory, dirName);

        return currentDirectory;
    }

    private DirectoryInfo CreateSubDirectory(DirectoryInfo currentDirectory, string dirName)
    {
        if (_directoryService.TryCreateSubDirectoryIfNotExist(currentDirectory, dirName, out var subDirectory) &&
            subDirectory != null) return subDirectory;

        OnErrorThrown?.Invoke($"Failed to create or access subdirectory {dirName} in {currentDirectory.FullName}.");
        return currentDirectory;
    }
}