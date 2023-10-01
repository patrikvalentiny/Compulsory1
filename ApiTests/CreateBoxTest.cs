using System.Net.Http.Json;
using Bogus;
using Dapper;
using FluentAssertions.Execution;
using infrastructure.Models;
using Newtonsoft.Json;
using Npgsql;

namespace TestAPI;

public class CreateBoxTests
{
    private Box _box;
    private HttpClient _httpClient;
    private NpgsqlDataSource _npgsql;
    private Box _response;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();

        Helper.TriggerRebuild();
        var boxes = new Faker<Box>()
            .StrictMode(true)
            .RuleFor(o => o.Width, f => f.Random.Decimal(max: 300m))
            .RuleFor(o => o.Height, f => f.Random.Decimal(max: 300m))
            .RuleFor(o => o.Depth, f => f.Random.Decimal(max: 300m))
            .RuleFor(o => o.Description, f => f.Lorem.Text())
            .RuleFor(o => o.Location, f => f.Address.FullAddress())
            .RuleFor(o => o.Guid, f => f.Random.Guid().OrNull(f, 1f))
            .RuleFor(o => o.Created, f => f.Date.Soon().OrNull(f, 1f));
        _box = boxes.Generate();
        _npgsql = Helper.CreateDataSource();
    }

    [Test]
    public async Task Test1()
    {
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
            responseObject.Description.Should().Be(_box.Description);
            responseObject.Width.Should().Be(_box.Width);
            responseObject.Height.Should().Be(_box.Height);
            responseObject.Depth.Should().Be(_box.Depth);
            responseObject.Location.Should().Be(_box.Location);
        }
    }

    [TearDown]
    public void TearDown()
    {
        using var conn = _npgsql.OpenConnection();
        conn.Execute("DELETE FROM box_factory.box_inventory WHERE guid = @guid", _response);
    }
}