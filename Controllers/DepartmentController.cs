using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/departments")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentRepository _repo;
    public DepartmentController(IDepartmentRepository repo) => _repo = repo;
    
    [HttpGet]
    public async Task<IActionResult> Get([FromHeader(Name="X-Compound-ID")] int compoundId)
        => Ok(await _repo.GetByCompoundAsync(compoundId));
    
    [HttpPost]
    public async Task<IActionResult> Post([FromHeader(Name="X-Compound-ID")] int compoundId, [FromBody] CreateDepartmentRequest req)
    {
        if (string.IsNullOrEmpty(req.Title)) return BadRequest(new { error = "Title is required" });
        var dept = await _repo.CreateAsync(req.Title, compoundId);
        return Ok(dept);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id, [FromHeader(Name="X-Compound-ID")] int compoundId)
    {
        var dept = await _repo.GetByIdAsync(id, compoundId);
        if (dept==null) return NotFound();
        return Ok(dept);
    }
} 