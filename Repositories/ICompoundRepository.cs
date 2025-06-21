public interface ICompoundRepository
{
    Task<IEnumerable<Compound>> GetAllAsync();
    Task<Compound> CreateAsync(string title);
} 