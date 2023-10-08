namespace ApiTests;

public class CreateBoxTests
{
    private HttpClient _httpClient;
    private BoxWithMaterialId box;
    private Faker<BoxWithMaterialId> _boxFaker;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        List<int> materials;

        using var conn = Helper.DataSource.OpenConnection();
        var sql = @$"SELECT id as {nameof(BoxWithMaterialId.MaterialId)} FROM box_factory.materials";
        materials = conn.Query<int>(sql).ToList();


        _boxFaker = new Faker<BoxWithMaterialId>()
            .StrictMode(true)
            .RuleFor(o => o.Width, f => f.Random.Decimal(min: 0.01m, max: 300m))
            .RuleFor(o => o.Height, f => f.Random.Decimal(min: 0.01m, max: 300m))
            .RuleFor(o => o.Depth, f => f.Random.Decimal(min: 0.01m, max: 300m))
            .RuleFor(o => o.Description, f => f.Lorem.Text())
            .RuleFor(o => o.Location, f => f.Address.FullAddress())
            .RuleFor(o => o.Guid, f => f.Random.Guid())
            .RuleFor(o => o.Created, f => f.Date.Soon())
            .RuleFor(o => o.Title, f => f.Lorem.Sentence())
            .RuleFor(o => o.Quantity, f => f.Random.Int(min: 1, max: 1000))
            .RuleFor(o => o.MaterialId, f => f.PickRandom<int>(materials));
        
        box = _boxFaker.Generate();
    }

    [Test]
    public async Task CreateBox()
    {
        var url = "http://localhost:5000/api/boxes";

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsJsonAsync(url, box);
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
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
            (await Helper.IsCorsFullyEnabledAsync(url)).Should().BeTrue();
            response.IsSuccessStatusCode.Should().BeTrue();
            responseObject.Should().BeEquivalentTo(box, options => options.Excluding(o => o.Created));
        }
    }

    [TearDown]
    public void TearDown()
    {
        using var conn = Helper.DataSource.OpenConnection();
        conn.Execute(@$"DELETE FROM box_factory.box_inventory WHERE guid = @{nameof(BoxWithMaterialId.Guid)}", box);
    }
}