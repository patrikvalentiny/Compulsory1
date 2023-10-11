using infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using service;

namespace api.Controllers;

[ApiController]
[Route("api/materials")]
public class MaterialStorageController : Controller
{
    private readonly MaterialService _service;
    
    public MaterialStorageController(MaterialService materialService)
    {
        _service = materialService;
    }
    [HttpGet]
    public IActionResult GetAllMaterials()
    {
        return Ok(_service.GetAllMaterials());
    }

    [HttpGet("{id}")]
    public IActionResult GetMaterialById([FromRoute] int id)
    {
        return Ok(_service.GetMaterialById(id));
    }
    
    [HttpPost]
    public IActionResult CreateMaterial([FromBody] Material material)
    {
        return Ok(_service.CreateMaterial(material));
    }
    
    [HttpPut("{id}")]
    public IActionResult UpdateMaterial([FromBody] Material material, [FromRoute] int id)
    {
        return Ok(_service.UpdateMaterial(material));
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteMaterial([FromRoute] int id)
    {
        return Ok(_service.DeleteMaterial(id));
    }
}