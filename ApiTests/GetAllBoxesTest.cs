namespace ApiTests;

public class GetAllBoxesTest
{
    private HttpClient _httpClient;
    private List<Box> _boxes = new ();
    private int _boxCount = 20;
    
    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        Helper.TriggerRebuild();
        
        var boxes = new Faker<Box>()
            .StrictMode(true)
            .RuleFor(o => o.Width, f => f.Random.Decimal(min:0.01m, max: 300m))
            .RuleFor(o => o.Height, f => f.Random.Decimal(min:0.01m, max: 300m))
            .RuleFor(o => o.Depth, f => f.Random.Decimal(min:0.01m, max: 300m))
            .RuleFor(o => o.Description, f => f.Lorem.Text())
            .RuleFor(o => o.Location, f => f.Address.FullAddress())
            .RuleFor(o => o.Guid, f => f.Random.Guid())
            .RuleFor(o => o.Created, f => f.Date.Soon());

        using (var conn = Helper.DataSource.OpenConnection())
        {
            for (int i = 0; i < _boxCount; i++)
            {
                const string sql = @$"INSERT INTO box_factory.box_inventory (guid, width, height, depth, location, description, datetime_created) 
        VALUES (@{nameof(Box.Guid)}, @{nameof(Box.Width)}, @{nameof(Box.Height)}, @{nameof(Box.Depth)}, @{nameof(Box.Location)}, @{nameof(Box.Description)}, @{nameof(Box.Created)})
        RETURNING 
            guid as {nameof(Box.Guid)}, 
            width as {nameof(Box.Width)}, 
            height as {nameof(Box.Height)}, 
            depth as {nameof(Box.Depth)}, 
            location as {nameof(Box.Location)}, 
            description as {nameof(Box.Description)},
            datetime_created as {nameof(Box.Created)}
        ";
                Box box = boxes.Generate();
                _boxes.Add(box);
                conn.Execute(sql, box);
            }
        }
    }

    [Test]
    public async Task CompareSizeInputToReturn()
    {
        var url = "http://localhost:5000/api/boxes";

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(url);
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }

        catch (Exception e)
        {
            throw new Exception(Helper.NoResponseMessage, e);
        }

        Box[] responseObject;
        try
        {
            responseObject = JsonConvert.DeserializeObject<Box[]>(
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
            responseObject.Length.Should().Be(_boxCount);
            responseObject.Should().BeEquivalentTo(_boxes, options => options.Excluding(o => o.Created));
        }    
    }

    [TearDown]
    public void TearDown()
    {
        Helper.TriggerRebuild();
    }
}