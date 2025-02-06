using Otus_Homework_9.Services;

namespace Otus_Homework_9;

internal class Program
{
    private const string RootPath = @"C:\";

    private static readonly IDirectoryService _directoryService = new DirectoryService();
    private static readonly IFileService _fileService = new FileService();
    private static readonly IPathService _pathService = new PathService(_directoryService);

    private static void Main(string[] args)
    {
        _directoryService.OnErrorThrown += HandleError;
        _fileService.OnErrorThrown += HandleError;

        var fileNames = GenerateFileNames(10);

        var directoryPaths = new List<string>
        {
            @"Otus\TestDir1",
            @"Otus\TestDir2"
        };

        foreach (var path in directoryPaths)
        {
            var filePathCollection = CreateDirectoryWithFiles(RootPath, path, fileNames);
            if (filePathCollection.Count > 0) WriteToFiles(filePathCollection);
        }

        foreach (var path in directoryPaths) PrintFilesInfo(RootPath, path, fileNames);

        Console.ReadKey();

        _directoryService.OnErrorThrown -= HandleError;
        _fileService.OnErrorThrown -= HandleError;
    }

    private static List<string> GenerateFileNames(int count)
    {
        const int counter = 1;

        var fileNames = new List<string>();
        for (var i = counter; i <= count; i++)
        {
            var fileName = $"File{i}.txt";
            fileNames.Add(fileName);
        }

        return fileNames;
    }

    private static List<FileInfo> CreateDirectoryWithFiles(string rootPath, string pathString, List<string> fileNames)
    {
        var directoryPath = _pathService.CreatePath(RootPath, pathString);
        return _fileService.TryCreateFilesIfNotExisted(directoryPath, fileNames);
    }

    private static void WriteToFiles(List<FileInfo> filePathCollection)
    {
        foreach (var file in filePathCollection)
        {
            _fileService.WriteToFile(file, file.Name);
            _fileService.WriteToFile(file, DateTime.Now.ToString());
        }
    }

    private static void PrintFilesInfo(string rootPath, string directoryPath, List<string> fileNames)
    {
        var directory = new DirectoryInfo(Path.Combine(rootPath, directoryPath));

        if (!directory.Exists)
        {
            Console.WriteLine($"Directory {directory.FullName} not exist.");
            return;
        }

        Console.WriteLine($"Files in directory: {directory.FullName}");

        foreach (var fileName in fileNames)
        {
            var filePath = Path.Combine(directory.FullName, fileName);
            var file = new FileInfo(filePath);

            if (!file.Exists)
            {
                Console.WriteLine($"File {file.Name} not found in {directory.FullName}");
                continue;
            }

            try
            {
                var content = File.ReadAllLines(file.FullName);
                Console.WriteLine($"{file.Name}:");
                Console.WriteLine(string.Join("\n", content));
                Console.WriteLine();
            }
            catch (Exception exception)
            {
                HandleError($"Unable to read file {file.Name}: {exception.Message}");
            }
        }
    }


    private static void HandleError(string errorMessage)
    {
        Console.WriteLine(errorMessage);
    }
}