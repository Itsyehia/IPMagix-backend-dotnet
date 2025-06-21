using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/compounds")]
public class CompoundController : ControllerBase
{
    private readonly ICompoundRepository _repo;
    public CompoundController(ICompoundRepository repo) => _repo = repo;
    
    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _repo.GetAllAsync());
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateCompoundRequest req)
    {
        if (string.IsNullOrEmpty(req.Title)) return BadRequest(new { error = "Title is required" });
        var compound = await _repo.CreateAsync(req.Title);
        return Ok(compound);
    }
} 