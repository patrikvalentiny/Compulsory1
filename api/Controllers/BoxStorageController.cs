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
        return Ok(_service.GetAllBoxes());
    }
    
    [HttpGet ("{guid}")]
    public IActionResult GetBoxByGuid([FromRoute]Guid guid)
    {
        try
        {
            return Ok(_service.GetBoxByGuid(guid));
        }
        catch (InvalidOperationException e)
        {
            return NotFound("Box does not exist");
        }
        
    }
    
    [HttpPost("")]
    public IActionResult CreateBox([FromBody] Box box)
    {
        return Ok(_service.CreateBox(box));
    }
    
    [HttpPut("")]
    public IActionResult UpdateBox([FromBody] Box box)
    {
        return Ok(box);
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
