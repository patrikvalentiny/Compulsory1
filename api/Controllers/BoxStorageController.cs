using infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using service;

namespace api.Controllers;

[ApiController]
[Route("api/boxes")]
public class BoxStorageController : Controller
{
    private readonly Service _service;
    
    public BoxStorageController(Service service)
    {
        _service = service;
    }
    
    [HttpGet ("")]
    public IActionResult GetAllBoxes()
    {
        return Ok();
    }
    
    [HttpGet ("{guid}")]
    public IActionResult GetBoxByGuid([FromRoute]Guid guid)
    {
        return Ok();
    }
    
    [HttpPost("")]
    public IActionResult CreateBox([FromBody] Box box)
    {
        return Ok(box);
    }
    
    [HttpPut("")]
    public IActionResult UpdateBox([FromBody] Box box)
    {
        return Ok();
    }
    
    [HttpDelete("{guid}")]
    public IActionResult DeleteBox([FromRoute] Guid guid)
    {
        try
        {
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
}
