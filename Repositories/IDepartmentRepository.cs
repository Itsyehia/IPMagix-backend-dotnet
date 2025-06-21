public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> GetByCompoundAsync(int compoundId);
    Task<Department> CreateAsync(string title, int compoundId);
    Task<Department> GetByIdAsync(int id, int compoundId);
} 