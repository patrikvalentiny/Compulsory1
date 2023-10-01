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

    public Box CreateBox(Box box)
    {
        return _repository.CreateBox(box);
    }
}