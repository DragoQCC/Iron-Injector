using Alba.CsConsoleFormat;
using Iron_Injector.Commands.Program_Commands;
using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static Iron_Injector.Models.Data;

namespace Iron_Injector.Models.Abstracts
{
    using static System.ConsoleColor; // used by table to draw with colors.
    public abstract class ModuleBase
    {       
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract int ID { get; }
        public abstract string Prompt { get; }

        protected static ModuleBase CurrentModule { get; set; }

        public static List<Command> ModuleCommands { get; set; } // list of commands that can be ran from module like help,clear,info,set,run
        public static List<OptionsBase> ModuleOptions { get; set; } // options to be set in the modules, such as the url, filename, allocation type, or whatever else the method might be cabable of doing 

        public void Init() // Calls our abstract functions to get values from child classes, then loads into interactive prompt. called in ui class when module is detected in input
        {
            setModuleCommands();
            setModuleOptions();
            CurrentModule = this; // sets to whatever the called class is so other classes / methods can know what module we are working inside of 
            string input;
            Program.prompt = Prompt;
            Console.WriteLine($"{"[*]".Pastel(Color.DeepSkyBlue)}Loading {Name.Pastel(Color.Green)} module, use help for commands and options to see what the module can do.");

            while(true)
            {
                Console.Write($"{Program.prompt}");
                input = Console.ReadLine();

                if (input == "exit")
                {
                    if (Program.prompt == "[ProgramLoader]> ")
                    {
                        ExecuteAssembly.fixSpy(); // fixes what fodhelper broke
                    }
                    break;
                }
                UI.ui.callCommand(input);
            }
        }
        public abstract void setModuleOptions(); // forces child classes to set these so we have values for the table
        public abstract void setModuleCommands();
        public abstract void ActivateRunCommand();

        public class Table
        {
            public static void MakeTable()
            {
                LineThickness headerThickness = new LineThickness(LineWidth.Single, LineWidth.Single);
                Document OptionsTable = new Document
                (

                    new Grid
                    {
                        Color = Gray,
                        Columns = { GridLength.Auto, GridLength.Auto, GridLength.Auto, GridLength.Auto },
                        Children =
                        {
                            new Cell("Name") { Stroke = headerThickness },
                            new Cell("Current Value") { Stroke = headerThickness },
                            new Cell("Is Required") { Stroke = headerThickness },
                            new Cell("Description") { Stroke = headerThickness },

                            ModuleOptions.Select(listOfOptions => new []
                            {
                                new Cell{Children = { listOfOptions.Name } },
                                new Cell{Children = { listOfOptions.CurrentValue } },
                                new Cell{Children = { listOfOptions.IsRequired } },
                                new Cell{Children = { listOfOptions.Description } },

                            })
                        }
                    }
                );
                ConsoleRenderer.RenderDocument((Document)OptionsTable);
            }
        }
        public class Set : Command
        {
            public override string Name => "set";
            public override string Description => "Used to set a value for one of the module options, args required. ex. set url 127.0.0.1";
            public bool didUpdate;

            public override void ExecuteCommand(string[] args)
            {
                if (Program.prompt == Program.OrginalPrompt)
                {
                    Console.WriteLine($"{"[-]".Pastel(Color.Red)}Select a module before setting options.");
                    return;
                }

                if (args[0] == "set")
                {
                    foreach (var currentModuleOptions in ModuleOptions)
                    {
                        if (args.Count() < 3)
                        {
                            break; 
                        }
                        if (currentModuleOptions.Name.Equals(args[1], StringComparison.OrdinalIgnoreCase) == true && args[2] != null)
                        {
                            Console.WriteLine($"{"[+]".Pastel(Color.Green)} updating {currentModuleOptions.Name} to {args[2]}");
                            currentModuleOptions.CurrentValue = args[2];
                            didUpdate = true;
                            break;
                        }
                        else
                        {
                            didUpdate = false;
                        }
                    }

                    if (didUpdate == false)
                    {
                        Console.WriteLine($"{"[-]".Pastel(Color.Red)} {args[1]} is not an available option.");
                    }
                }
                else
                {
                    Console.WriteLine(args[0]);
                }
            }
        }
        public class run : Command // run is a command so I can keep track of commands and error when user enters a non command 
        {
            public Dictionary<string,bool?> isExecuted = new Dictionary<string, bool?>();
            public override string Name => "Run";

            public override string Description => "starts the module with the current options";

            public override void ExecuteCommand(string[] args)
            {
                bool? IsExecute = null;
                if (Program.prompt == Program.OrginalPrompt)
                {
                    Console.WriteLine($"{"[-]".Pastel(Color.Red)}Select a module & fill in options before using run.");
                    return ;
                }
                foreach(var currentModuleOptions in ModuleOptions )
                {
                    if (currentModuleOptions.IsRequired == true && String.IsNullOrWhiteSpace(currentModuleOptions.CurrentValue) == true)
                    {
                        Console.WriteLine($"{"[-]".Pastel(Color.Red)}{currentModuleOptions.Name.Pastel(Color.DeepSkyBlue)} must be set to a value before running");
                        IsExecute = false;
                        isExecuted.Add(currentModuleOptions.Name, IsExecute);
                    }
                    else
                    {
                        IsExecute = true;
                        isExecuted[currentModuleOptions.Name] = IsExecute; // should update dictyionay value for key which is option name.
                        
                    }
                    
                }
                foreach (bool execute in isExecuted.Values) // makes sure all options have a current value set so execution can work and if not stops before execution finishes. 
                {
                    if (execute == false)
                    {
                        return;
                    }
                }
                Console.WriteLine($"{"[+]".Pastel(Color.Green)}Executing {ModuleBase.CurrentModule.Name} module");
                ModuleBase.CurrentModule.ActivateRunCommand();
            }
            
        }
    }
}
