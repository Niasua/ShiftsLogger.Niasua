using ShiftsLogger.ConsoleUI.Models;
using Spectre.Console;

namespace ShiftsLogger.ConsoleUI.UI;

public static class Display
{
    public static void ShowWorkers(List<Worker> workers)
    {
        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.AddColumn("[yellow]Id[/]");
        table.AddColumn("[blue]Name[/]");
        table.AddColumn("[green]Job[/]");

        foreach (var worker in workers)
        {
            table.AddRow(
                worker.Id.ToString(),
                worker.Name.ToString(),
                worker.Job.ToString()
                );
        }

        AnsiConsole.Write(table);
    }

    public static void ShowWorker(Worker worker)
    {
        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.AddColumn("[yellow]ID[/]");
        table.AddColumn("[blue]Name[/]");
        table.AddColumn("[green]Job[/]");

        table.AddRow(
                 worker.Id.ToString(),
                 worker.Name.ToString(),
                 worker.Job.ToString()
                 );

        AnsiConsole.Write(table);
    }

    public static Worker? PromptSelectWorker(List<Worker> workers)
    {
        if (workers == null || workers.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[red]No workers available.[/]");
            return null;
        }

        var backOption = new Worker { Id = -1, Name = "[Back]", Job = "" };

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<Worker>()
            .Title("[green]\nSelect a worker:[/]")
            .PageSize(10)
            .UseConverter(w =>
                w.Id == -1
                    ? "[grey]Back[/]"
                    : $"[green]{w.Id}[/] - [green]{w.Name}[/] - [green]{w.Job}[/]")
            .AddChoices(workers.Append(backOption))
        );

        return selected.Id == -1 ? null : selected;
    }

    public static async Task ShowShiftsAsync(List<Shift> shifts)
    {
        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.AddColumn("[yellow]Start[/]");
        table.AddColumn("[blue]End[/]");
        table.AddColumn("[green]Type[/]");
        table.AddColumn("[cyan]Worker[/]");

        var apiService = new ApiService();

        foreach (var shift in shifts)
        {
            var worker = await apiService.GetWorkerByIdAsync(shift.WorkerId);

            table.AddRow(
                shift.Start.ToString("g"),
                shift.End.ToString("g"),
                shift.Type.ToString(),
                worker?.Name ?? "[Unknown]"
            );
        }

        AnsiConsole.Write(table);
    }

    public static async Task ShowShiftAsync(Shift shift)
    {
        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.AddColumn("[yellow]Start[/]");
        table.AddColumn("[blue]End[/]");
        table.AddColumn("[green]Type[/]");
        table.AddColumn("[cyan]Worker[/]");

        var apiService = new ApiService();

        var worker = await apiService.GetWorkerByIdAsync(shift.WorkerId);

        table.AddRow(
            shift.Start.ToString("g"),
            shift.End.ToString("g"),
            shift.Type.ToString(),
            worker?.Name ?? "[Unknown]"
        );

        AnsiConsole.Write(table);
    }

    public static DateTime? PromptSelectDateTime()
    {
        string zzz = "zzz";

        string yearInput = AnsiConsole.Ask<string>("Year (or 'zzz' to cancel):");
        if (yearInput.ToLower() == zzz) return null;
        if (!int.TryParse(yearInput, out int year) || year < DateTime.Now.Year)
        {
            AnsiConsole.MarkupLine("[red]Invalid year. Must be this year or later.[/]");
            return PromptSelectDateTime();
        }

        var month = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select month (or [grey]zzz[/] to cancel):")
                .AddChoices(Enumerable.Range(1, 12).Select(m => m.ToString("00")).Prepend(zzz)));

        if (month == zzz) return null;

        var day = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select day (or [grey]zzz[/] to cancel):")
                .AddChoices(Enumerable.Range(1, DateTime.DaysInMonth(year, int.Parse(month)))
                                      .Select(d => d.ToString("00"))
                                      .Prepend(zzz)));

        if (day == zzz) return null;

        var hour = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select hour (or [grey]zzz[/] to cancel):")
                .AddChoices(Enumerable.Range(0, 24)
                                      .Select(h => h.ToString("00"))
                                      .Prepend(zzz)));

        if (hour == zzz) return null;

        var minute = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select minute (or [grey]zzz[/] to cancel):")
                .AddChoices(Enumerable.Range(0, 60)
                                      .Select(m => m.ToString("00"))
                                      .Prepend(zzz)));

        if (minute == zzz) return null;

        return new DateTime(
            year,
            int.Parse(month),
            int.Parse(day),
            int.Parse(hour),
            int.Parse(minute),
            0
        );
    }

    public static async Task<Shift?> PromptSelectShiftAsync(List<Shift> shifts)
    {
        if (shifts == null || shifts.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[red]No shifts available.[/]");
            return null;
        }

        var apiService = new ApiService();
        var allWorkers = await apiService.GetAllWorkersAsync();
        var workerDict = allWorkers.ToDictionary(w => w.Id, w => w.Name);

        var shiftChoices = shifts
            .Select(s =>
            {
                var workerName = workerDict.ContainsKey(s.WorkerId) ? workerDict[s.WorkerId] : "[Unknown]";
                var label = $"{s.Start:g} - {s.End:g} - {s.Type} - {workerName}";
                return new { Shift = s, Label = label };
            })
            .ToList();

        shiftChoices.Insert(0, new { Shift = (Shift?)null, Label = "[grey]Back[/]" });

        var selectedLabel = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]\nSelect a shift:[/]")
                .PageSize(10)
                .AddChoices(shiftChoices.Select(c => c.Label))
        );

        var selected = shiftChoices.FirstOrDefault(c => c.Label == selectedLabel)?.Shift;

        return selected;
    }
}
