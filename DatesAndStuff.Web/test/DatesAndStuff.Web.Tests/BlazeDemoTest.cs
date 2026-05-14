using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace DatesAndStuff.Web.Tests;

[TestFixture]
public class BlazeDemoTest
{
    private IWebDriver driver;
    private const string BaseURL = "https://blazedemo.com/";

    [SetUp]
    public void SetUp()
    {
        driver = new ChromeDriver();
    }

    [TearDown]
    public void TearDown()
    {
        try { driver.Quit(); driver.Dispose(); } catch { }
    }

    [Test]
    public void BlazeDemo_MexicoCityToDublin_ShouldHaveAtLeastThreeFlights()
    {
        // Arrange
        driver.Navigate().GoToUrl(BaseURL);

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        var fromDropdown = new SelectElement(
            wait.Until(d => d.FindElement(By.Name("fromPort"))));
        var toDropdown = new SelectElement(
            wait.Until(d => d.FindElement(By.Name("toPort"))));

        // Act
        fromDropdown.SelectByText("Mexico City");
        toDropdown.SelectByText("Dublin");

        driver.FindElement(By.CssSelector("input.btn-primary")).Click();

        // Assert
        var flightRows = wait.Until(d =>
        {
            var rows = d.FindElements(
                By.XPath("//table[contains(@class,'table')]/tbody/tr"));
            return rows.Count > 0 ? rows : null;
        });

        flightRows.Count.Should().BeGreaterThanOrEqualTo(3,
            because: "mexico -> dublin should have more flights");
    }
}