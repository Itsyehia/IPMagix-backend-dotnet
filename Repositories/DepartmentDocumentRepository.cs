using Dapper;

public class DepartmentDocumentRepository : IDepartmentDocumentRepository
{
    private readonly DBConnectionManager _db;
    public DepartmentDocumentRepository(DBConnectionManager db) => _db = db;
    
    public async Task<IEnumerable<Document>> GetDocumentsForDepartmentAsync(int departmentId, int compoundId)
    {
        using var conn = _db.GetConnection();
        const string sql = @"
SELECT d.id AS Id, d.title AS Title, d.url AS Url, d.size AS Size, d.sha256 AS Sha256, d.status AS Status, d.index_name AS IndexName, d.indexer_name AS IndexerName
FROM ipx_documents d
JOIN ipx_departments_documents dd ON d.id = dd.document_id
WHERE dd.department_id = @DepartmentId AND dd.compound_id = @CompoundId
ORDER BY d.id";
        return await conn.QueryAsync<Document>(sql, new { DepartmentId = departmentId, CompoundId = compoundId });
    }
    
    public async Task AssignAsync(int departmentId, int documentId)
    {
        using var conn = _db.GetConnection();
        const string sql = "INSERT INTO ipx_departments_documents (department_id, document_id, compound_id) VALUES (@DepartmentId, @DocumentId, (SELECT compound_id FROM ipx_departments WHERE id = @DepartmentId))";
        await conn.ExecuteAsync(sql, new { DepartmentId = departmentId, DocumentId = documentId });
    }
} 