using Microsoft.AspNetCore.Mvc;
using service;

namespace api.Controllers;

[ApiController]
[Route("api")]
public class BoxStorageController : Controller
{
    private readonly Service _service;
    
    public BoxStorageController(Service service)
    {
        _service = service;
    }
    [HttpGet ("boxes")]
    public IActionResult GetAllBoxes()
    {
        return Ok();
    }
}
