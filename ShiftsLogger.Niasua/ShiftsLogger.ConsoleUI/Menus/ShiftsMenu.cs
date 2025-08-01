using ShiftsLogger.ConsoleUI.Models;
using ShiftsLogger.ConsoleUI.UI;
using Spectre.Console;

namespace ShiftsLogger.ConsoleUI.Menus;

public class ShiftsMenu
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
                .Title("[blue]Shifts Menu[/]")
                .AddChoices(new[]
                {
                    "Show All Shifts", "Show Shift by Worker ID", "Show Shift by ID" ,"Create Shift", "Edit Shift", "Remove Shift", "Back"
                }));

            switch (option)
            {
                case "Show All Shifts":

                    await ShowAllShifts();

                    break;

                case "Show Shift by Worker ID":

                    await ShowShiftByWorkerId();

                    break;

                case "Show Shift by ID":

                    await ShowShiftById();

                    break; 

                case "Create Shift":

                    await CreateShift();

                    break;

                case "Edit Shift":

                    await EditShift();

                    break;

                case "Remove Shift":

                    await RemoveShift();

                    break;

                case "Back":

                    exit = true;

                    break;
            }
        }
    }

    private static async Task ShowAllShifts()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[green]View Shifts\n[/]");

        var shifts = await ApiService.GetAllShiftsAsync();

        if (shifts == null)
        {
            AnsiConsole.MarkupLine("[red]The shifts could not be found.[/]");
        }
        else
        {
            await Display.ShowShiftsAsync(shifts);
        }

        AnsiConsole.MarkupLine("\n[grey]Press any key to go back...[/]");
        Console.ReadKey();

        Console.Clear();
    }
    private static async Task ShowShiftByWorkerId()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[green]View Worker's Shifts\n[/]");

        var workers = await ApiService.GetAllWorkersAsync();

        var worker = Display.PromptSelectWorker(workers);
        if (worker == null) return;

        var shifts = await ApiService.GetShiftsByWorkerIdAsync(worker.Id);

        if (shifts == null || shifts.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[red]The shifts could not be found.[/]");
        }
        else
        {
            await Display.ShowShiftsAsync(shifts);
        }

        AnsiConsole.MarkupLine("\n[grey]Press any key to go back...[/]");
        Console.ReadKey();

        Console.Clear();
    }
    private static async Task ShowShiftById()
    {
        while (true)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[green]View Shift\n[/]");

            var input = AnsiConsole.Ask<string>("Type shift's [green]ID[/] ('zzz' to return): ");

            if (input.Trim().ToLower() == "zzz") return;

            if (!int.TryParse(input, out int id))
            {
                AnsiConsole.MarkupLine("\n[red]Please enter a valid number or 'zzz' to return.[/]");
                Console.ReadKey();
                continue;
            }

            var shift = await ApiService.GetShiftByIdAsync(id);

            if (shift == null)
            {
                AnsiConsole.MarkupLine("\n[red]The shift could not be found.[/]");
            }
            else
            {
                await Display.ShowShiftAsync(shift);
            }

            AnsiConsole.MarkupLine("\n[grey]Press any key to go back...[/]");
            Console.ReadKey();

            Console.Clear(); 
        }
    }
    private static async Task CreateShift()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[green]Create Shift ('zzz' to return)\n[/]");

        DateTime? start, end;

        while(true)
        {
            AnsiConsole.MarkupLine("[green]Shift start:[/]");
            start = Display.PromptSelectDateTime();
            if (start == null) return;
            AnsiConsole.MarkupLine($"\n[green]You've typed {start:g}[/]\n");

            AnsiConsole.MarkupLine("[green]Shift end:[/]");
            end = Display.PromptSelectDateTime();
            if (end == null) return;
            AnsiConsole.MarkupLine($"\n[green]You've typed {end:g}[/]\n");

            if (end <= start)
            {
                AnsiConsole.MarkupLine("[red]End time must be after start time. Try again.[/]\n");
            }
            else
            {
                break;
            }
        }

        var choices = Enum.GetValues<ShiftType>()
                  .Select(t => t.ToString())
                  .Prepend("zzz")
                  .ToList();

        var typeStr = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("\nSelect type (or [grey]zzz[/] to cancel):")
                .AddChoices(choices));

        if (typeStr == "zzz")
            return; 

        var type = Enum.Parse<ShiftType>(typeStr);

        var workers = await ApiService.GetAllWorkersAsync();
        var worker = Display.PromptSelectWorker(workers);
        if (worker == null) return;

        var created = await ApiService.CreateShiftAsync(new Shift { Start = (DateTime)start, End = (DateTime)end, Type = type, WorkerId = worker.Id });

        if (created == null)
        {
            AnsiConsole.MarkupLine("\n[red]Shift creation was not possible.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("\n[green]Shift was successfully created.[/]");
        }

        AnsiConsole.MarkupLine("\n[grey]Press any key to go back...[/]");
        Console.ReadKey();

        Console.Clear();
    }
    private static async Task EditShift()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[green]Edit Shift\n[/]");

        var shifts = await ApiService.GetAllShiftsAsync();
        var shift = await Display.PromptSelectShiftAsync(shifts);
        if (shift == null) return;

        var newShift = new Shift
        {
            Id = shift.Id
        };

        // Start Time
        var startTimeInput = AnsiConsole.Prompt(
            new TextPrompt<bool>($"Want to change Start Time ({shift.Start:g})?")
                .AddChoice(true)
                .AddChoice(false)
                .DefaultValue(false)
                .WithConverter(choice => choice ? "y" : "n"));

        if (startTimeInput)
        {
            newShift.Start = (DateTime)Display.PromptSelectDateTime();
        }
        else
        {
            newShift.Start = shift.Start;
        }

        // End Time
        var endTimeInput = AnsiConsole.Prompt(
            new TextPrompt<bool>($"Want to change End Time ({shift.End:g})?")
                .AddChoice(true)
                .AddChoice(false)
                .DefaultValue(false)
                .WithConverter(choice => choice ? "y" : "n"));

        if (endTimeInput)
        {
            newShift.End = (DateTime)Display.PromptSelectDateTime();
        }
        else
        {
            newShift.End = shift.End;
        }

        // Type
        var typeInput = AnsiConsole.Prompt(
                    new TextPrompt<bool>($"Want to change Type ({shift.Type.ToString()})?")
                        .AddChoice(true)
                        .AddChoice(false)
                        .DefaultValue(false)
                        .WithConverter(choice => choice ? "y" : "n"));

        if (typeInput)
        {
            var type = AnsiConsole.Prompt(
                new SelectionPrompt<ShiftType>()
                .Title("\nSelect type:")
                .AddChoices(Enum.GetValues<ShiftType>())
                );

            newShift.Type = type;
        }
        else
        {
            newShift.Type = shift.Type;
        }

        // Worker
        var worker = await ApiService.GetWorkerByIdAsync(shift.WorkerId);

        var workerInput = AnsiConsole.Prompt(
                    new TextPrompt<bool>($"Want to change Worker ({worker.Name})?")
                        .AddChoice(true)
                        .AddChoice(false)
                        .DefaultValue(false)
                        .WithConverter(choice => choice ? "y" : "n"));

        if (workerInput)
        {
            var workers = await ApiService.GetAllWorkersAsync();
            worker = Display.PromptSelectWorker(workers);
            if (worker == null) return;

            newShift.WorkerId = worker.Id;
        }
        else
        {
            newShift.WorkerId = shift.WorkerId;
        }

        var updated = await ApiService.UpdateShiftAsync(shift.Id, newShift);

        if (!updated)
        {
            AnsiConsole.MarkupLine("\n[red]Shift edit was not possible.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("\n[green]Shift was successfully edited.[/]");
        }

        AnsiConsole.MarkupLine("\n[grey]Press any key to go back...[/]");
        Console.ReadKey();

        Console.Clear();
    }
    private static async Task RemoveShift()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[green]Remove Shift\n[/]");

        var shifts = await ApiService.GetAllShiftsAsync();
        var shift = await Display.PromptSelectShiftAsync(shifts);
        if (shift == null) return;

        var confirmation = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"Are you sure you want to [red]Remove[/] the Shift?")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down)[/]")
                .AddChoices(new[] {
                    "Yes", "No"
        }));

        if (confirmation == "Yes")
        {
            await ApiService.DeleteShiftAsync(shift.Id);
            AnsiConsole.MarkupLine("\n[green]Shift was successfully removed.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("\n[red]Shift removal was not possible.[/]");
        }

        AnsiConsole.MarkupLine("\n[grey]Press any key to go back...[/]");
        Console.ReadKey();

        Console.Clear();
    }
}
