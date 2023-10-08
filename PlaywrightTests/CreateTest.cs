using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PlaywrightTests;

public class CreateTests : PageTest
{
    [SetUp]
    public void Setup()
    {
        Page.GotoAsync("http://localhost:4200/add");
    }

    [Test]
    public async Task Test1()
    {
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
        
        var submitButton = Page.GetByTestId("submitButton");
        await Expect(submitButton).ToBeEnabledAsync();
        await submitButton.ClickAsync();

        var confirmationAlert = Page.GetByTestId("confirmationAlert");
        await Expect(confirmationAlert).ToHaveCSSAsync("opacity","1");
        await Expect(confirmationAlert).ToHaveCSSAsync("opacity","0");
    }
    
    [TearDown]
    public void TearDown()
    {
        Page.CloseAsync();
        
    }
}