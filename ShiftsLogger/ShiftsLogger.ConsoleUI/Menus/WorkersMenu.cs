using ShiftsLogger.ConsoleUI.Models;
using ShiftsLogger.ConsoleUI.UI;
using Spectre.Console;

namespace ShiftsLogger.ConsoleUI.Menus;

public class WorkersMenu
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
                .Title("[blue]Workers Menu[/]")
                .AddChoices(new[]
                {
                    "Show All Workers", "Show Worker by ID", "Create Worker", "Edit Worker", "Remove Worker", "Back"
                }));

            switch (option)
            {
                case "Show All Workers":

                    await ShowAllWorkers();

                    break;

                case "Show Worker by ID":

                    await ShowWorkerById();

                    break;

                case "Create Worker":

                    await CreateWorker();

                    break;

                case "Edit Worker":

                    await EditWorker();

                    break;

                case "Remove Worker":

                    await RemoveWorker();

                    break;

                case "Back":

                    exit = true;

                    break;
            }
        }
    }

    private async static Task ShowAllWorkers()
    {
        while (true)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[green]View Workers\n[/]");

            var workers = await ApiService.GetAllWorkersAsync();

            if (workers == null)
            {
                AnsiConsole.MarkupLine("\n[red]The workers could not be found.[/]");
            }
            else
            {
                Display.ShowWorkers(workers);
            }

            AnsiConsole.MarkupLine("\n[grey]Press any key to go back...[/]");
            Console.ReadKey();

            Console.Clear();
            break;
        }
    }
    private static async Task ShowWorkerById()
    {
        while (true)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[green]View Worker ('zzz' to return)\n[/]");

            var input = AnsiConsole.Ask<string>("Type worker's [green]ID[/] or 'zzz' to return:");

            if (input.Trim().ToLower() == "zzz")
                return;

            if (!int.TryParse(input, out int workerId))
            {
                AnsiConsole.MarkupLine("\n[red]Please enter a valid number or 'zzz' to return.[/]");
                Console.ReadKey();
                continue;
            }

            var worker = await ApiService.GetWorkerByIdAsync(workerId);

            if (worker == null)
            {
                AnsiConsole.MarkupLine("\n[red]The worker could not be found.[/]");
            }
            else
            {
                Display.ShowWorker(worker);
            }

            AnsiConsole.MarkupLine("\n[grey]Press any key to go back...[/]");
            Console.ReadKey();

            Console.Clear();
            break;
        }

    }
    private static async Task CreateWorker()
    {
        while (true)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[green]Create Worker ('zzz' to return)\n[/]");

            var name = AnsiConsole.Prompt(
                new TextPrompt<string>("Type worker's [green]Name[/]:")
                    .Validate(input =>
                        string.IsNullOrWhiteSpace(input)
                        ? ValidationResult.Error("[red]Name cannot be empty[/]")
                        : ValidationResult.Success()
                        )
                    );
            if (name == "zzz") break;

            var job = AnsiConsole.Ask<string>("Type worker's [green]Job[/]: ");
            if (job == "zzz") break;

            var worker = await ApiService.CreateWorkerAsync(new Worker { Name = name, Job = job });

            if (worker == null)
            {
                AnsiConsole.MarkupLine("\n[red]Worker creation was not possible.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("\n[green]Worker was successfully created.[/]");
            }

            AnsiConsole.MarkupLine("\n[grey]Press any key to go back...[/]");
            Console.ReadKey();

            Console.Clear();
            break;
        }
    }
    private static async Task EditWorker()
    {
        while (true)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[green]Edit Worker ('zzz' to return)\n[/]");

            var workers = await ApiService.GetAllWorkersAsync();

            var worker = Display.PromptSelectWorker(workers);
            if (worker == null) return;

            var name = AnsiConsole.Prompt(
                new TextPrompt<string>($"Type worker's [green]Name[/] ([grey]{worker.Name}[/]):")
                    .AllowEmpty()
            );
            if (name == "zzz") break;

            var job = AnsiConsole.Prompt(
                new TextPrompt<string>($"Type worker's [green]Job[/] ([grey]{worker.Job}[/]):")
                    .AllowEmpty()
            );
            if (job == "zzz") break;

            var updatedWorker = new Worker
            {
                Id = worker.Id,
                Name = string.IsNullOrWhiteSpace(name) ? worker.Name : name,
                Job = string.IsNullOrWhiteSpace(job) ? worker.Job : job
            };

            var updated = await ApiService.UpdateWorkerAsync(worker.Id, updatedWorker);

            if (!updated)
            {
                AnsiConsole.MarkupLine("\n[red]Worker edit was not possible.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("\n[green]Worker was successfully edited.[/]");
            }

            AnsiConsole.MarkupLine("\n[grey]Press any key to go back...[/]");
            Console.ReadKey();

            Console.Clear();
            break;
        }
    }
    private static async Task RemoveWorker()
    {
        while (true)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[green]Remove Worker\n[/]");

            var workers = await ApiService.GetAllWorkersAsync();

            var worker = Display.PromptSelectWorker(workers);
            if (worker == null) return;

            var removed = await ApiService.DeleteWorkerAsync(worker.Id);

            if (!removed)
            {
                AnsiConsole.MarkupLine("\n[red]Worker removal was not possible.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("\n[green]Worker was successfully removed.[/]");
            }

            AnsiConsole.MarkupLine("\n[grey]Press any key to go back...[/]");
            Console.ReadKey();

            Console.Clear();
            break;
        }
    }
}
