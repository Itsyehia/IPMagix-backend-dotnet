using Dapper;

public class DocumentRepository : IDocumentRepository
{
    private readonly DBConnectionManager _db;
    public DocumentRepository(DBConnectionManager db) => _db = db;
    
    public async Task<IEnumerable<Document>> GetByCompoundAsync(int compoundId)
    {
        using var conn = _db.GetConnection();
        const string sql = @"SELECT id AS Id, title AS Title, url AS Url, size AS Size, sha256 AS Sha256, status AS Status, index_name AS IndexName, indexer_name AS IndexerName FROM ipx_documents WHERE compound_id = @CompoundId ORDER BY id";
        return await conn.QueryAsync<Document>(sql, new { CompoundId = compoundId });
    }
} 