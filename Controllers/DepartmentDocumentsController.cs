using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/departments/{departmentId}/documents")]
public class DepartmentDocumentsController : ControllerBase
{
    private readonly IDepartmentDocumentRepository _repo;
    public DepartmentDocumentsController(IDepartmentDocumentRepository repo) => _repo = repo;
    
    [HttpGet]
    public async Task<IActionResult> Get(int departmentId, [FromHeader(Name="X-Compound-ID")] int compoundId)
        => Ok(await _repo.GetDocumentsForDepartmentAsync(departmentId, compoundId));
    
    [HttpPost]
    public async Task<IActionResult> Post(int departmentId, [FromBody] AssignDocumentRequest req)
    {
        await _repo.AssignAsync(departmentId, req.DocumentId);
        return Ok(new { message = $"Document {req.DocumentId} assigned to department {departmentId}" });
    }
} 