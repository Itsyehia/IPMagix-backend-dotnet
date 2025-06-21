public interface IDepartmentDocumentRepository
{
    Task<IEnumerable<Document>> GetDocumentsForDepartmentAsync(int departmentId, int compoundId);
    Task AssignAsync(int departmentId, int documentId);
} 