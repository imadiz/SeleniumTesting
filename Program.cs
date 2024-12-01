using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using static SeleniumTesting.MyConsoleWriter;

ChromeOptions options = new();
options.AddArgument("--start-maximized");
options.AddArgument("detach=false");
options.BinaryLocation = $"./";


ChromeDriver driver = new ChromeDriver(options);

WriteInfo("ChromeDriver előkészítve!");

CancellationTokenSource tokenSource = new CancellationTokenSource();
CancellationToken token = tokenSource.Token;
List<Task> All_Tasks = new();
void Start()
{
    string nav_to = "https://www.selenium.dev/selenium/web/web-form.html";
    WriteInfo($"Navigálás a következő oldalra: {nav_to}");
    driver.Navigate().GoToUrl(nav_to);
    WriteInfo($"Az oldal betöltött!");
    WriteInfo($"Az oldal címe: '{driver.Title}'");
    Task.Run(() =>
    {
        WriteToTextBox();
    });
    Task.Run(() =>
    {
        SelectOptions();
    });
    Task.Delay(5000).Wait();
    Stop();
}
void WriteToTextBox()
{
    WriteInfo("TextBox megkeresése...");
    IWebElement textbox = driver.FindElement(By.Name("my-text"));
    WriteInfo("Textbox megtalálva!");

    WriteInfo("Írás megkezdése a textboxba...");
    Task WritingToTextBox = Task.Run(() =>
    {
        string to_write = "Ez a szöveg!";
        int charindex = 0;
        while (true)
        {
            if (charindex.Equals(to_write.Length))
            {
                Task.Delay(1000).Wait();
                charindex = 0;
                textbox.Clear();
                if (token.IsCancellationRequested)
                {
                    WriteInfo($"WritingToTextBox(Id: {Task.CurrentId}) feladat leállítva.");
                    return;
                }
            }
            textbox.SendKeys(to_write[charindex].ToString());
            charindex++;
            Task.Delay(100).Wait();
        }
    });
    All_Tasks.Add(WritingToTextBox);
}
void SelectOptions()
{
    WriteInfo("Választó megkeresése...");
    SelectElement select = new(driver.FindElement(By.Name("my-select")));
    WriteInfo("Választó megtalálva!");
    IList<IWebElement> SelectOptions = select.Options;
    WriteInfo($"Választási lehetőségek: [{string.Join(", ", SelectOptions.Select(x=>x.Text))}]");
    WriteInfo("Választgatás megkezdése...");
    Task SelectInCombobox = Task.Run(() =>
    {
        int selectindex = 0;
        while (true)
        {
            if (selectindex.Equals(SelectOptions.Count)) 
            {
                selectindex = 0;
                if (token.IsCancellationRequested)
                {
                    WriteInfo($"SelectInCombobox(Id: {Task.CurrentId}) feladat leállítva.");
                    return;
                }
            }
            select.SelectByIndex(selectindex);
            selectindex++;
            Task.Delay(500).Wait();
        }
    });
    All_Tasks.Add(SelectInCombobox);
}
void Stop()
{
    Console.WriteLine();
    WriteInfo("Feladatok leállítása...");
    tokenSource.Cancel();
    Task.WaitAll([.. All_Tasks]);
    WriteInfo("Az összes feladat leállt!");
    Console.BackgroundColor = ConsoleColor.Red;
    Console.ForegroundColor = ConsoleColor.Black;

    Console.Write("Leállás 5mp múlva. ");
    for (int i = 5; i > 0; i--)
    {
        Console.Write($"{i}...");
        Task.Delay(1000).Wait();
    }

    driver.Quit();
}

Start();

WriteInfo("Nyomd meg egy billentyűt a leálláshoz...");
Console.ReadKey();
Stop();