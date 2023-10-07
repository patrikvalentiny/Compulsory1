using infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using service;

namespace api.Controllers;

[ApiController]
[Route("api/boxes")]
public class BoxStorageController : Controller
{
    private readonly BoxService _boxService;
    
    public BoxStorageController(BoxService boxService)
    {
        _boxService = boxService;
    }
    
    [HttpGet ("")]
    public IActionResult GetAllBoxes()
    {
        return Ok(_boxService.GetAllBoxes());
    }
    
    [HttpGet ("{guid}")]
    public IActionResult GetBoxByGuid([FromRoute]Guid guid)
    {
        try
        {
            return Ok(_boxService.GetBoxByGuid(guid));
        }
        catch (InvalidOperationException e)
        {
            return NotFound("Box does not exist");
        }
        
    }
    
    [HttpPost("")]
    public IActionResult CreateBox([FromBody] BoxWithMaterialId box)
    {
        return Ok(_boxService.CreateBox(box));
    }
    
    [HttpPut("")]
    public IActionResult UpdateBox([FromBody] BoxWithMaterialId box)
    {
        return Ok(_boxService.UpdateBox(box));
    }
    
    [HttpDelete("{guid}")]
    public IActionResult DeleteBox([FromRoute] Guid guid)
    {
        try
        {
            _boxService.DeleteBox(guid);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
}
