using Spectre.Console;

namespace ShiftsLogger.ConsoleUI.Menus;

public static class MainMenu
{
    public static ApiService ApiService { get; set; } = new();

    public static async Task Show()
    {
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[blue]Shifts Logger Menu[/]")
                .AddChoices(new[]
                {
                    "Workers Menu", "Shifts Menu", "Exit"
                }));

            switch (option)
            {
                case "Workers Menu":

                    await WorkersMenu.Show();

                    break;

                case "Shifts Menu":

                    await ShiftsMenu.Show();

                    break;

                case "Exit":

                    exit = true;

                    break;
            }
        }
    }
}

