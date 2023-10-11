using infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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

    [HttpGet("")]
    public IActionResult GetAllBoxes()
    {
        try
        {
            return Ok(_boxService.GetAllBoxes());
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            return BadRequest();
        }
    }

    [HttpGet("feed")]
    public IActionResult GetFeed()
    {
        return Ok(_boxService.GetFeed());
    }

    [HttpGet("{guid}")]
    public IActionResult GetBoxByGuid([FromRoute] Guid guid)
    {
        try
        {
            return Ok(_boxService.GetBoxByGuid(guid));
        }
        catch (InvalidOperationException)
        {
            return NotFound("Box does not exist");
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            return BadRequest();
        }
    }

    [HttpPost("")]
    public IActionResult CreateBox([FromBody] BoxWithMaterialId box)
    {
        try
        {
            return Ok(_boxService.CreateBox(box));
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            return BadRequest();
        }
    }

    [HttpPut("")]
    public IActionResult UpdateBox([FromBody] BoxWithMaterialId box)
    {
        try
        {
            return Ok(_boxService.UpdateBox(box));
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            return BadRequest();
        }
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
            Log.Error(e.Message);
            return BadRequest(e.Message);
        }
    }
}