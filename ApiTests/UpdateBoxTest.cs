namespace ApiTests;

public class UpdateBoxTest
{
    private HttpClient _httpClient;
    private BoxWithMaterialId box;
    private BoxWithMaterialId updateBox;
    private Faker<BoxWithMaterialId> boxes;
    [SetUp]
    public void SetUp()
    {
        _httpClient = new HttpClient();

        List<int> materials;

        using var conn = Helper.DataSource.OpenConnection();
        var sql = @$"SELECT id FROM box_factory.materials";
        materials = conn.Query<int>(sql).ToList();
        
        
        boxes = new Faker<BoxWithMaterialId>()
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
        updateBox = boxes.Generate();
        updateBox.Guid = box.Guid;
        conn.Execute(sql, box);
    }
    
    [Test]
    public async Task TestReturnIsInput()
    {
        var url = $@"http://localhost:5000/api/boxes";
        
        HttpResponseMessage response;
        try
        {
            response = _httpClient.PutAsJsonAsync(url, updateBox).Result;
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + response.Content.ReadAsStringAsync().Result);
        }

        catch (Exception e)
        {
            throw new Exception(Helper.NoResponseMessage, e);
        }
        
        BoxWithMaterialId responseObject;
        try
        {
            responseObject = JsonConvert.DeserializeObject<BoxWithMaterialId>(
                await response.Content.ReadAsStringAsync()) ?? throw new InvalidOperationException();
        }
        catch (Exception e)
        {
            throw new Exception(Helper.BadResponseBody(await response.Content.ReadAsStringAsync()), e);
        }

        using (new AssertionScope())
        {
            updateBox.Should().BeEquivalentTo(responseObject, options => options.Excluding(o => o.Created));
        }
    }
    
    [TearDown]
    public void TearDown()
    {
        using var conn = Helper.DataSource.OpenConnection();
        var sql = @$"DELETE FROM box_factory.box_inventory WHERE guid = @{nameof(Box.Guid)}";
        conn.Execute(sql, box);
    }
}