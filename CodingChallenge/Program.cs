using System.Xml.Linq;

while (true)
{
    var directoryPath = "./input-data"; // TODO: get from appsettings
    if (!Directory.Exists(directoryPath))
    {
        Console.WriteLine("Could not find specified directory.");
        Console.WriteLine("Please make sure you have set the directory name in appsettings.");
        Console.WriteLine("Exiting...");
        return;
    }
    var filePaths = Directory.GetFiles(directoryPath);
    foreach (var filePath in filePaths)
    {
        if (!filePath.ToLower().EndsWith(".xml")) continue;
        
        var xmlFile = XElement.Load(filePath);
        
        // TODO: do stuff with loaded xml
        
    }
}
