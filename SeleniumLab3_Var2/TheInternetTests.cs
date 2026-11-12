using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumLab3_Var2;

public class Tests
{
    private IWebDriver driver;
    private readonly string baseUrl = "https://the-internet.herokuapp.com/";
    [SetUp]
    public void Setup()
    {
        driver = new EdgeDriver(); 
        driver.Manage().Window.Maximize(); 
    }

    [Test]
    public void ForgotPassword_ShouldAcceptEmailAndSubmit()
    {
        const string targetUrl = "https://the-internet.herokuapp.com/forgot_password";
        const string expectedEmail = "test@example.com";
        const string emailFieldId = "email";
        const string submitButtonId = "form_submit";

        driver.Navigate().GoToUrl(targetUrl);

        var emailInput = driver.FindElement(By.Id(emailFieldId));
        emailInput.Clear();
        emailInput.SendKeys(expectedEmail);

        Assert.That(emailInput.GetAttribute("value"), Is.EqualTo(expectedEmail));

        var retrievePasswordButton = driver.FindElement(By.Id(submitButtonId));
        retrievePasswordButton.Click();

        Assert.That(driver.Url, Is.Not.EqualTo(targetUrl));
    }
    
    [Test]
    public void HorizontalSlider_ShouldMoveAndDisplayCorrectValue()
    {
        const string targetUrl = "https://the-internet.herokuapp.com/horizontal_slider";
        const string sliderSelector = "input[type='range']";
        const string rangeId = "range";
        const string expectedValue = "0.5";

        driver.Navigate().GoToUrl(targetUrl);

        var slider = driver.FindElement(By.CssSelector(sliderSelector));
        var rangeValue = driver.FindElement(By.Id(rangeId));

        slider.SendKeys(Keys.ArrowRight);

        Assert.That(rangeValue.Text, Is.EqualTo(expectedValue));
    }
    
    [Test]
    public void Dropdown_ShouldSelectOption()
    {
        const string targetUrl = "https://the-internet.herokuapp.com/dropdown";
        const string dropdownId = "dropdown";
        const string expectedText = "Option 2";

        driver.Navigate().GoToUrl(targetUrl);

        var dropdownElement = driver.FindElement(By.Id(dropdownId));
        var selectElement = new SelectElement(dropdownElement);

        selectElement.SelectByText(expectedText);

        Assert.That(selectElement.SelectedOption.Text, Is.EqualTo(expectedText));
    }
    
    [Test]
    public void Typos_ShouldNotHaveTypoInParagraph()
    {
        const string targetUrl = "https://the-internet.herokuapp.com/typos";
        const string paragraphXPath = "//div[@class='example']/p[2]";
        const string expectedText = "Sometimes you'll see a typo, other times you won't.";

        driver.Navigate().GoToUrl(targetUrl);

        var paragraph = driver.FindElement(By.XPath(paragraphXPath));

        Assert.That(paragraph.Text, Is.EqualTo(expectedText));
    }
    
    [Test]
    public void EntryAd_ShouldCloseModalWindow()
    {
        const string targetUrl = "https://the-internet.herokuapp.com/entry_ad";
        const string modalId = "modal";
        const string closeButtonXPath = "//div[@class='modal-footer']/p";
        const string displayProperty = "display";
        const string noneValue = "none";

        driver.Navigate().GoToUrl(targetUrl);

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
    
        var closeButton = wait.Until(d => 
        {
            var el = d.FindElement(By.XPath(closeButtonXPath));
            if (el.Displayed && el.Enabled) return el;
            return null;
        });
    
        System.Threading.Thread.Sleep(1000);
        closeButton.Click();

        var modal = driver.FindElement(By.Id(modalId));
        wait.Until(d => modal.GetCssValue(displayProperty) == noneValue);
        var displayStyle = modal.GetCssValue(displayProperty);

        Assert.That(displayStyle, Is.EqualTo(noneValue));
    }
    
    [Test]
    public void FileDownload_ShouldDownloadFile()
    {
        const string targetUrl = "https://the-internet.herokuapp.com/download";
        const string fileLinkSelector = ".example a";

        driver.Navigate().GoToUrl(targetUrl);

        var fileLink = driver.FindElement(By.CssSelector(fileLinkSelector));
    
        fileLink.Click();

        Assert.That(fileLink.Displayed, Is.True);
    }
    
    [Test]
    public void BasicAuth_ShouldAuthenticateWithCredentialsInUrl()
    {
        const string targetUrl = "https://admin:admin@the-internet.herokuapp.com/basic_auth";
        const string successMessageSelector = ".example p";
        const string expectedMessage = "Congratulations! You must have the proper credentials.";

        driver.Navigate().GoToUrl(targetUrl);

        var messageElement = driver.FindElement(By.CssSelector(successMessageSelector));

        Assert.That(messageElement.Text, Is.EqualTo(expectedMessage));
    }
    
    [Test]
    public void DynamicLoading_ShouldDisplayHiddenElementAfterLoading()
    {
        const string targetUrl = "https://the-internet.herokuapp.com/dynamic_loading/1";
        const string startButtonSelector = "#start button";
        const string finishTextId = "finish";
        const string expectedText = "Hello World!";

        driver.Navigate().GoToUrl(targetUrl);

        var startButton = driver.FindElement(By.CssSelector(startButtonSelector));
        startButton.Click();

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        var finishElement = wait.Until(d => 
        {
            var el = d.FindElement(By.Id(finishTextId));
            if (el.Displayed) return el;
            return null;
        });

        Assert.That(finishElement.Text, Is.EqualTo(expectedText));
    }
    
    [Test]
    public void ContextMenu_ShouldTriggerAlertOnRightClick()
    {
        const string targetUrl = "https://the-internet.herokuapp.com/context_menu";
        const string hotSpotId = "hot-spot";
        const string expectedAlertText = "You selected a context menu";

        driver.Navigate().GoToUrl(targetUrl);

        var hotSpot = driver.FindElement(By.Id(hotSpotId));
        var actions = new OpenQA.Selenium.Interactions.Actions(driver);
    
        actions.ContextClick(hotSpot).Perform();

        var alert = driver.SwitchTo().Alert();
        var alertText = alert.Text;
        alert.Accept();

        Assert.That(alertText, Is.EqualTo(expectedAlertText));
    }
    
    [Test]
    public void RedirectLink_ShouldRedirectToStatusCodesPage()
    {
        const string targetUrl = "https://the-internet.herokuapp.com/redirector";
        const string redirectLinkId = "redirect";
        const string expectedRedirectUrl = "https://the-internet.herokuapp.com/status_codes";

        driver.Navigate().GoToUrl(targetUrl);

        var redirectLink = driver.FindElement(By.Id(redirectLinkId));
        redirectLink.Click();

        Assert.That(driver.Url, Is.EqualTo(expectedRedirectUrl));
    }
    
    [TearDown]
    public void Teardown()
    {
        driver.Quit(); 
        driver.Dispose();
    }
}