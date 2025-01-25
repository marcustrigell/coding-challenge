using System.Globalization;
using System.Xml;
using CodingChallenge.Models;

namespace CodingChallenge;

public class GenerationReportCalculator
{
    private readonly CultureInfo _cultureInfo = CultureInfo.CreateSpecificCulture("en-GB");
    
    private readonly List<string> _generationTypes =
    [
        "Wind",
        "Gas",
        "Coal"
    ];

    private readonly XmlDocument _xmlFile;
    
    private List<GenerationDay> _generationDays = [];
    private decimal? _emissionRating;
    private decimal? _totalHeatInput;
    private decimal? _actualNetGeneration;

    public GenerationReportCalculator(string filePath)
    {
        _xmlFile = new XmlDocument();
        _xmlFile.Load(filePath);
    }

    public void ParseGenerationReport(string filePath)
    {
        foreach (var type in _generationTypes)
        {
            HandleGeneratorData(_xmlFile, type);
        }
    }
    
    private void HandleGeneratorData(XmlDocument xmlFile, string generatorType)
    {
        var generators = xmlFile.GetElementsByTagName(generatorType + "Generator");
        
        foreach (XmlElement generator in generators)
        {
            var name = generator.GetElementsByTagName("Name")[0]?.InnerText;
            if (name == null)
            {
                Console.WriteLine("Could not parse name of generator, continuing to next generator.");
                continue;
            }
            
            var generation = generator.GetElementsByTagName("Generation");
            foreach (XmlElement day in generation)
            {
                HandleGenerationDay(name, day);
            }
            
            var emissionsRatingString = generator.GetElementsByTagName("EmissionsRating")[0]?.InnerText;
            _emissionRating = decimal.TryParse(emissionsRatingString, _cultureInfo, out var emissionsRating) ? emissionsRating : null;
            
            var totalHeatInputString = generator.GetElementsByTagName("TotalHeatInput")[0]?.InnerText;
            _totalHeatInput = decimal.TryParse(totalHeatInputString, _cultureInfo, out var totalHeatInput) ? totalHeatInput : null;
            
            var actualNetGenerationString = generator.GetElementsByTagName("ActualNetGeneration")[0]?.InnerText;
            _actualNetGeneration = decimal.TryParse(actualNetGenerationString, _cultureInfo, out var actualNetGeneration) ? actualNetGeneration : null;
        }
    }

    private void HandleGenerationDay(string generatorName, XmlElement day)
    {
        var dateString = day.GetElementsByTagName("Date")[0]?.InnerText;
        if (!DateTime.TryParse(dateString, out var date))
        {
            Console.WriteLine($"Could not parse date for generator {generatorName}");
            Console.WriteLine($"Date string: {dateString}");
            Console.WriteLine("Continuing to next generator day");
            return;
        }
                
        var energyString = day.GetElementsByTagName("Energy")[0]?.InnerText;
        if (!decimal.TryParse(energyString, _cultureInfo, out var energy))
        {
            Console.WriteLine($"Could not parse energy for generator {generatorName}");
            Console.WriteLine($"Day: {date.ToString()}");
            Console.WriteLine($"Energy string: {energyString}");
            Console.WriteLine("Continuing to next generator day");
            return;
        }
        
        var priceString = day.GetElementsByTagName("Price")[0]?.InnerText;
        if (!decimal.TryParse(priceString, _cultureInfo, out var price))
        {
            Console.WriteLine($"Could not parse price for generator {generatorName}");
            Console.WriteLine($"Day: {date.ToString()}");
            Console.WriteLine($"Price string: {priceString}");
            Console.WriteLine("Continuing to next generator day");
            return;
        }
                
        _generationDays.Add(new GenerationDay
        {
            GeneratorName = generatorName,
            Date = date,
            Energy = energy,
            Price = price,
        });
    }
}