using ApiTests;
using infrastructure.Models;

namespace PlaywrightTests;

public class CreateTests : PageTest
{
    private BoxWithMaterialId? _response;
    [SetUp]
    public void Setup()
    {
        Page.GotoAsync("http://localhost:5000");
    }

    [Test]
    public async Task TestCreateBox()
    {
        await Page.GetByTestId("createBoxLink").ClickAsync();
        await Page.GetByTestId("titleInput").FillAsync("TestTitle");
        await Page.GetByTestId("widthInput").FillAsync("10");
        await Page.GetByTestId("heightInput").FillAsync("10");
        await Page.GetByTestId("depthInput").FillAsync("10");
        await Page.GetByTestId("locationInput").FillAsync("location");
        await Page.GetByTestId("descriptionInput").FillAsync("TestDescription");
        await Page.GetByTestId("quantityInput").FillAsync("10");
        
        var materialsButton = Page.GetByTestId("materialsDropdownButton");
        await Expect(materialsButton).ToBeEnabledAsync();
        await materialsButton.HoverAsync();
        
        await Expect(Page.GetByTestId("materialsDropdown")).ToBeVisibleAsync();
        await Page.Locator("ul[data-testid='materialsDropdown'] > li:nth-of-type(1)").ClickAsync();
        
        var waitForResponseTask = Page.WaitForResponseAsync("http://localhost:5000/api/boxes");
        
        var submitButton = Page.GetByTestId("submitButton");
        await Expect(submitButton).ToBeEnabledAsync();
        await submitButton.ClickAsync();
        
        _response = JsonConvert.DeserializeObject<BoxWithMaterialId>(await waitForResponseTask.Result.TextAsync());

        var confirmationAlert = Page.GetByTestId("confirmationAlert");
        await Expect(confirmationAlert).ToHaveCSSAsync("opacity","1");
        await Expect(confirmationAlert).ToHaveCSSAsync("opacity","0");
    }
    
    [TearDown]
    public void TearDown()
    {
        Page.CloseAsync();
        using var conn = Helper.DataSource.OpenConnection();
        if (_response == null)
        {
            conn.Execute("DELETE FROM box_factory.box_inventory WHERE datetime_created = (SELECT max(datetime_created) FROM box_factory.box_inventory)");
        }
        else
        {
            conn.Execute("DELETE FROM box_factory.box_inventory WHERE guid = @guid", new {guid = _response?.Guid});
        }
        
    }
}