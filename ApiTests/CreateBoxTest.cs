namespace ApiTests;

public class CreateBoxTests
{
    private Box _box;
    private HttpClient _httpClient;
    private Box _response;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();

        Helper.TriggerRebuild();
    }

    [Test]
    public async Task CreateBox()
    {
        var boxes = new Faker<Box>()
            .StrictMode(true)
            .RuleFor(o => o.Width, f => f.Random.Decimal(min: 0.01m, max: 300m))
            .RuleFor(o => o.Height, f => f.Random.Decimal(min: 0.01m, max: 300m))
            .RuleFor(o => o.Depth, f => f.Random.Decimal(min: 0.01m, max: 300m))
            .RuleFor(o => o.Description, f => f.Lorem.Text())
            .RuleFor(o => o.Location, f => f.Address.FullAddress())
            .RuleFor(o => o.Guid, f => f.Random.Guid().OrNull(f, 1f))
            .RuleFor(o => o.Created, f => f.Date.Soon().OrNull(f, 1f))
            .RuleFor(o => o.Title, f => f.Lorem.Sentence());
        _box = boxes.Generate();
        
        var url = "http://localhost:5000/api/boxes";

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsJsonAsync(url, _box);
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
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
            _response = responseObject;
        }
        catch (Exception e)
        {
            throw new Exception(Helper.BadResponseBody(await response.Content.ReadAsStringAsync()), e);
        }

        using (new AssertionScope())
        {
            (await Helper.IsCorsFullyEnabledAsync(url)).Should().BeTrue();
            response.IsSuccessStatusCode.Should().BeTrue();
            responseObject.Should().BeEquivalentTo(_box, options => options.Excluding(o => o.Guid).Excluding(o => o.Created));
        }
    }

    [TearDown]
    public void TearDown()
    {
        using var conn = Helper.DataSource.OpenConnection();
        conn.Execute("DELETE FROM box_factory.box_inventory WHERE guid = @guid", _response);
    }
}