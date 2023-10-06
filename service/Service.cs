using infrastructure;
using infrastructure.Models;

namespace service;

public class Service
{
    private readonly Repository _repository;

    public Service(Repository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Box> GetAllBoxes()
    {
        return _repository.GetAllBoxes();
    }

    public Box GetBoxByGuid(Guid guid)
    {
        return _repository.GetBoxByGuid(guid);
    }

    public BoxWithMaterialId CreateBox(BoxWithMaterialId box)
    {
        return _repository.CreateBox(box);
    }

    public BoxWithMaterialId UpdateBox(BoxWithMaterialId box)
    {
        return _repository.UpdateBox(box);
    }

    public int DeleteBox(Guid guid)
    {
        return _repository.DeleteBox(guid);
    }
}