// See https://aka.ms/new-console-template for more information

using ApiTests;
using Bogus;
using Dapper;
using infrastructure.Models;

SetUp();
void SetUp(int boxCount = 20)
{
    List<int> materials;
    BoxWithMaterialId? box;
    using var conn = Helper.DataSource.OpenConnection();
    var sql = @"SELECT id FROM box_factory.materials";
    materials = conn.Query<int>(sql).ToList();


    var boxes = new Faker<BoxWithMaterialId>()
        .StrictMode(true)
        .RuleFor(o => o.Width, f => f.Random.Decimal(0.01m, 300m))
        .RuleFor(o => o.Height, f => f.Random.Decimal(0.01m, 300m))
        .RuleFor(o => o.Depth, f => f.Random.Decimal(0.01m, 300m))
        .RuleFor(o => o.Description, f => f.Lorem.Text())
        .RuleFor(o => o.Location, f => f.Address.FullAddress())
        .RuleFor(o => o.Guid, f => f.Random.Guid())
        .RuleFor(o => o.Created, f => f.Date.Soon())
        .RuleFor(o => o.Title, f => f.Lorem.Sentence())
        .RuleFor(o => o.Quantity, f => f.Random.Int(1, 1000))
        .RuleFor(o => o.MaterialId, f => f.PickRandom(materials));

    sql =
        @$"INSERT INTO box_factory.box_inventory (guid, title, width, height, depth, location, description, quantity, material_id) 
        VALUES (@{nameof(BoxWithMaterialId.Guid)}, @{nameof(BoxWithMaterialId.Title)}, @{nameof(BoxWithMaterialId.Width)}, @{nameof(BoxWithMaterialId.Height)}, @{nameof(BoxWithMaterialId.Depth)}, @{nameof(BoxWithMaterialId.Location)}, @{nameof(BoxWithMaterialId.Description)}, @{nameof(BoxWithMaterialId.Quantity)}, @{nameof(BoxWithMaterialId.MaterialId)})";
    for (var i = 0; i < boxCount; i++)
    {
        box = boxes.Generate();
        conn.Execute(sql, box);
    }
}