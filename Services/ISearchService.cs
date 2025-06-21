public interface ISearchService
{
    Task<SearchResult> SearchDepartmentDocumentsAsync(int departmentId, string query, IEnumerable<int> documentIds, int compoundId);
} 