namespace Otus_Homework_9.Services;

public interface IFileService
{
    public event Action<string>? OnErrorThrown;
    List<FileInfo> TryCreateFilesIfNotExisted(DirectoryInfo directory, List<string> fileNames);
    bool WriteToFile(FileInfo file, string content);
}