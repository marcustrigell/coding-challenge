namespace CodingChallenge.Models;

public struct GenerationDay
{
    public required string GeneratorName { get; init; }
    public required DateTime Date { get; init; }
    public required decimal Energy  { get; init; }
    public required decimal Price { get; init; }
}