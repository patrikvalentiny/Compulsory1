using ApiTests;
using infrastructure.Models;

namespace PlaywrightTests;

public class DeleteTest: PageTest
{
    private BoxWithMaterialId box;
    [SetUp]
    public void Setup()
    {
        List<int> materials;

        using var conn = Helper.DataSource.OpenConnection();
        var sql = @$"SELECT id FROM box_factory.materials";
        materials = conn.Query<int>(sql).ToList();
        
        
        var boxes = new Faker<BoxWithMaterialId>()
            .StrictMode(true)
            .RuleFor(o => o.Width, f => f.Random.Decimal(min:0.01m, max: 300m))
            .RuleFor(o => o.Height, f => f.Random.Decimal(min:0.01m, max: 300m))
            .RuleFor(o => o.Depth, f => f.Random.Decimal(min:0.01m, max: 300m))
            .RuleFor(o => o.Description, f => f.Lorem.Text())
            .RuleFor(o => o.Location, f => f.Address.FullAddress())
            .RuleFor(o => o.Guid, f => f.Random.Guid())
            .RuleFor(o => o.Created, f => f.Date.Soon())
            .RuleFor(o => o.Title, f => f.Lorem.Sentence())
            .RuleFor(o => o.Quantity, f => f.Random.Int(min: 1, max: 1000))
            .RuleFor(o => o.MaterialId, f => f.PickRandom<int>(materials));
        
        sql = @$"INSERT INTO box_factory.box_inventory (guid, title, width, height, depth, location, description, quantity, material_id) 
        VALUES (@{nameof(BoxWithMaterialId.Guid)}, @{nameof(BoxWithMaterialId.Title)}, @{nameof(BoxWithMaterialId.Width)}, @{nameof(BoxWithMaterialId.Height)}, @{nameof(BoxWithMaterialId.Depth)}, @{nameof(BoxWithMaterialId.Location)}, @{nameof(BoxWithMaterialId.Description)}, @{nameof(BoxWithMaterialId.Quantity)}, @{nameof(BoxWithMaterialId.MaterialId)})";
        box = boxes.Generate();
        conn.Execute(sql, box);
        
        Page.GotoAsync("http://localhost:4200");
    }

    [Test]
    public async Task TestDeleteBox()
    {
        var l1 = Page.Locator(
            "div[data-testid='galleryWrapper'] > div:nth-of-type(1)");
        await l1.IsVisibleAsync();
        var l2 = l1.GetByTestId("deleteButton");
        await l2.ClickAsync();

        var l3 = Page.GetByTestId("confirmDeleteAlert");
        await l3.IsVisibleAsync();
        await Expect(l3).ToHaveCSSAsync("opacity", "1");
        await Expect(l3).ToHaveCSSAsync("opacity", "0");
    }
    
    [TearDown]
    public void TearDown()
    {
        Page.CloseAsync();
        using var conn = Helper.DataSource.OpenConnection();
        var sql = @$"DELETE FROM box_factory.box_inventory WHERE guid = @guid";
        conn.Execute(sql, new {guid = box.Guid});
        
    }
}