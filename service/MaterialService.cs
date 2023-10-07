using infrastructure;
using infrastructure.Models;

namespace service;

public class MaterialService
{
    private readonly MaterialsRepository _repository;
    
    public MaterialService(MaterialsRepository repository)
    {
        _repository = repository;
    }
    
    public IEnumerable<Material> GetAllMaterials()
    {
        return _repository.GetAllMaterials();
    }

    public Material GetMaterialById(int id)
    {
        return _repository.GetMaterialsById(id);
    }
    
    public Material CreateMaterial(Material material)
    {
        return _repository.CreateMaterial(material);
    }


    public Material UpdateMaterial(Material material)
    {
        return _repository.UpdateMaterial(material);
    }

    public int DeleteMaterial(int id)
    {
        return _repository.DeleteMaterial(id);
    }
}