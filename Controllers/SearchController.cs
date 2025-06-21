using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/departments/{departmentId}/search")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _search;
    public SearchController(ISearchService search) => _search = search;
    
    [HttpPost]
    public async Task<IActionResult> Post(int departmentId, [FromHeader(Name="X-Compound-ID")] int compoundId, [FromBody] SearchRequest req)
        => Ok(await _search.SearchDepartmentDocumentsAsync(departmentId, req.Query, req.DocumentIds, compoundId));
} 