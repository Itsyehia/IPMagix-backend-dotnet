using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/documents")]
public class DocumentController : ControllerBase
{
    private readonly IDocumentRepository _repo;
    public DocumentController(IDocumentRepository repo) => _repo = repo;
    
    [HttpGet]
    public async Task<IActionResult> Get([FromHeader(Name="X-Compound-ID")] int compoundId)
        => Ok(await _repo.GetByCompoundAsync(compoundId));
} 