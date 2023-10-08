namespace ApiTests;

public class GetSingleBoxTest
{
    private HttpClient _httpClient;
    private Box box;
    [SetUp]
    public void SetUp()
    {
        _httpClient = new HttpClient();
        Helper.TriggerRebuild();

        List<Material> materials;

        using var conn = Helper.DataSource.OpenConnection();
        var sql = @$"SELECT id as {nameof(Material.Id)}, material_name as {nameof(Material.Name)} FROM box_factory.materials";
        materials = conn.Query<Material>(sql).ToList();
        
        
        var boxes = new Faker<Box>()
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
            .RuleFor(o => o.Material, f => f.PickRandom<Material>(materials));
        
        sql = @$"INSERT INTO box_factory.box_inventory (guid, title, width, height, depth, location, description, quantity, material_id) 
        VALUES (@{nameof(Box.Guid)}, @{nameof(Box.Title)}, @{nameof(Box.Width)}, @{nameof(Box.Height)}, @{nameof(Box.Depth)}, @{nameof(Box.Location)}, @{nameof(Box.Description)}, @{nameof(Box.Quantity)}, @material_id)";
        box = boxes.Generate();
        conn.Execute(sql, new {guid = box.Guid, title = box.Title, width = box.Width, height = box.Height, depth = box.Depth, location = box.Location, description = box.Description, quantity = box.Quantity, material_id = box.Material.Id});
    }
    
    [Test]
    public async Task TestReturnIsInput()
    {
        var url = $@"http://localhost:5000/api/boxes/{box.Guid}";

        HttpResponseMessage response;
        try
        {
            response = _httpClient.GetAsync(url).Result;
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + response.Content.ReadAsStringAsync().Result);
        }

        catch (Exception e)
        {
            throw new Exception(Helper.NoResponseMessage, e);
        }
        
        Box responseObject;
        try
        {
            responseObject = JsonConvert.DeserializeObject<Box>(
                await response.Content.ReadAsStringAsync()) ?? throw new InvalidOperationException();
        }
        catch (Exception e)
        {
            throw new Exception(Helper.BadResponseBody(await response.Content.ReadAsStringAsync()), e);
        }

        using (new AssertionScope())
        {
            box.Should().BeEquivalentTo(responseObject, options => options.Excluding(o => o.Created));
        }
    }
    
    [TearDown]
    public void TearDown()
    {
        using var conn = Helper.DataSource.OpenConnection();
        var sql = @$"DELETE FROM box_factory.box_inventory WHERE guid = @guid";
        conn.Execute(sql, new {guid = box.Guid});
    }
}