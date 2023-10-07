using infrastructure;
using infrastructure.Models;

namespace service;

public class BoxService
{
    private readonly BoxRepository _boxRepository;

    public BoxService(BoxRepository boxRepository)
    {
        _boxRepository = boxRepository;
    }

    public IEnumerable<Box> GetAllBoxes()
    {
        return _boxRepository.GetAllBoxes();
    }

    public Box GetBoxByGuid(Guid guid)
    {
        return _boxRepository.GetBoxByGuid(guid);
    }

    public BoxWithMaterialId CreateBox(BoxWithMaterialId box)
    {
        return _boxRepository.CreateBox(box);
    }

    public BoxWithMaterialId UpdateBox(Box box)
    {
        return _boxRepository.UpdateBox(box);
    }

    public int DeleteBox(Guid guid)
    {
        return _boxRepository.DeleteBox(guid);
    }

    public IEnumerable<BoxOverviewItem> GetFeed()
    {
        return _boxRepository.GetFeed();
    }
}