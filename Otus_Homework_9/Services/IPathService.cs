namespace Otus_Homework_9.Services;

public interface IPathService
{
    public event Action<string>? OnErrorThrown;
    public DirectoryInfo CreatePath(string root, string path);
}