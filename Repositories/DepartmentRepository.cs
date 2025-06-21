using Dapper;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly DBConnectionManager _db;
    public DepartmentRepository(DBConnectionManager db) => _db = db;
    
    public async Task<IEnumerable<Department>> GetByCompoundAsync(int compoundId)
    {
        using var conn = _db.GetConnection();
        const string sql = "SELECT id AS Id, title AS Title, compound_id AS CompoundId FROM ipx_departments WHERE compound_id = @CompoundId ORDER BY id";
        return await conn.QueryAsync<Department>(sql, new { CompoundId = compoundId });
    }
    
    public async Task<Department> CreateAsync(string title, int compoundId)
    {
        using var conn = _db.GetConnection();
        const string sql = "INSERT INTO ipx_departments (title, compound_id) OUTPUT INSERTED.id, INSERTED.title, INSERTED.compound_id VALUES (@Title, @CompoundId)";
        return await conn.QuerySingleAsync<Department>(sql, new { Title = title, CompoundId = compoundId });
    }
    
    public async Task<Department> GetByIdAsync(int id, int compoundId)
    {
        using var conn = _db.GetConnection();
        const string sql = "SELECT id AS Id, title AS Title, compound_id AS CompoundId FROM ipx_departments WHERE id = @Id AND compound_id = @CompoundId";
        return await conn.QuerySingleOrDefaultAsync<Department>(sql, new { Id = id, CompoundId = compoundId });
    }
} 