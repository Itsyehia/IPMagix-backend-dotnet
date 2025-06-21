public interface IDocumentRepository
{
    Task<IEnumerable<Document>> GetByCompoundAsync(int compoundId);
} 