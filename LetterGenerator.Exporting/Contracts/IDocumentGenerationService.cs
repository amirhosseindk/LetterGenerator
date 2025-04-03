using LetterGenerator.Exporting.Models;

namespace LetterGenerator.Exporting.Contracts
{
    public interface IDocumentGenerationService
    {
        Task<string> GenerateWordAsync(ExportLetterDto letter);
        Task<string> GeneratePdfAsync(ExportLetterDto letter);
    }
}