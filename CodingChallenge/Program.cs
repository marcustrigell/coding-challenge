using System.Xml.Linq;

while (true)
{
    var directoryPath = "./input-data"; // TODO: get from appsettings
    var filePaths = Directory.GetFiles(directoryPath);
    foreach (var filePath in filePaths)
    {
        if (!filePath.ToLower().EndsWith(".xml")) continue;
        
        var xmlFile = XElement.Load(filePath);
        
        // TODO: do stuff with loaded xml
        
    }
}