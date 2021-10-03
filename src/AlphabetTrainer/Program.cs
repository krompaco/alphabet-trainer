using System;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AlphabetTrainer
{
    class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandApp<TrainingCommand>();
            return app.Run(args);
        }

        public class TrainingCommand : Command<TrainingCommand.Settings>
        {
            public override int Execute([NotNull] CommandContext context, [NotNull] Settings settingsParameter)
            {
                const string All = "ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖ";
                var all = All.ToCharArray();
                var progress = string.Empty;
                var success = true;

                AnsiConsole.WriteLine();
                AnsiConsole.Render(
                    new FigletText("TRÄNA ALFABETET")
                        .LeftAligned()
                        .Color(Color.Pink1));
                AnsiConsole.WriteLine();

                for (int i = 0; i < all.Length; i++)
                {
                    var question = i == 0
                        ? "Vilken bokstav kommer först i alfabetet?"
                        : $"{Environment.NewLine}Vilken bokstav kommer efter [bold yellow]{all[i - 1]}[/]?";

                    var letter = AnsiConsole.Ask<string>(question);

                    if (string.IsNullOrWhiteSpace(letter)
                        || letter.Length > 1
                        || !All.Contains(letter, StringComparison.CurrentCultureIgnoreCase))
                    {
                        i--;
                        continue;
                    }

                    if (!letter
                        .Trim()
                        .Equals(all[i].ToString(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        AnsiConsole.MarkupLine(progress + "[bold red]:([/]");
                        success = false;
                        break;
                    }

                    progress += "[bold green]:)[/] ";
                    AnsiConsole.MarkupLine(progress);
                }

                if (success)
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.Render(
                        new FigletText("ALLA RÄTT")
                            .LeftAligned()
                            .Color(Color.Yellow));
                    AnsiConsole.WriteLine();
                }
                else
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.Render(
                        new FigletText("FÖRSÖK IGEN")
                            .LeftAligned()
                            .Color(Color.Purple));
                    AnsiConsole.WriteLine();
                }

                return 0;
            }

            public class Settings : CommandSettings
            {
            }
        }
    }
}
