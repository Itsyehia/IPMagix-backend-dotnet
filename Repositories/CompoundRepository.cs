using Dapper;

public class CompoundRepository : ICompoundRepository
{
    private readonly DBConnectionManager _db;
    public CompoundRepository(DBConnectionManager db) => _db = db;
    
    public async Task<IEnumerable<Compound>> GetAllAsync()
    {
        using var conn = _db.GetConnection();
        const string sql = "SELECT id AS Id, title AS Title FROM ipx_compounds ORDER BY id";
        return await conn.QueryAsync<Compound>(sql);
    }
    
    public async Task<Compound> CreateAsync(string title)
    {
        using var conn = _db.GetConnection();
        const string sql = "INSERT INTO ipx_compounds (title) OUTPUT INSERTED.id, INSERTED.title VALUES (@Title)";
        return await conn.QuerySingleAsync<Compound>(sql, new { Title = title });
    }
} 