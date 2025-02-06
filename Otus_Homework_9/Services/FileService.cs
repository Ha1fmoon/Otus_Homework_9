using System.Text;

namespace Otus_Homework_9.Services;

public class FileService : IFileService
{
    public event Action<string>? OnErrorThrown;

    public List<FileInfo> TryCreateFilesIfNotExisted(DirectoryInfo directory, List<string> fileNames)
    {
        var files = new List<FileInfo>();
        try
        {
            foreach (var fileName in fileNames)
            {
                var response = TryCreateFile(directory, fileName);
                files.Add(response);
            }

            return files;
        }
        catch (Exception exception)
        {
            OnErrorThrown?.Invoke($"Cannot read or create file in {directory.FullName}, {exception.Message}");
            return new List<FileInfo>();
        }
    }

    private FileInfo TryCreateFile(DirectoryInfo directory, string fileName)
    {
        var filePath = Path.Combine(directory.FullName, fileName);

        if (File.Exists(filePath)) return new FileInfo(filePath);

        var file = File.Create(filePath);

        file.Close();

        return new FileInfo(file.Name);
    }

    public bool WriteToFile(FileInfo file, string content)
    {
        try
        {
            if (file.Exists)
            {
                using var fileWriter = new StreamWriter(file.FullName, true, Encoding.UTF8);
                fileWriter.WriteLine(content);
                fileWriter.Close();
                return true;
            }

            OnErrorThrown?.Invoke($"{file} not found in {file.DirectoryName}.");
            return false;
        }
        catch (Exception exception)
        {
            OnErrorThrown?.Invoke($"Cannot write to file {file.FullName}, {exception.Message}");
            return false;
        }
    }
}