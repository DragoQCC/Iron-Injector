using System;
using System.Collections.Generic;
using System.Linq;
using Pastel;
using System.Drawing;
using Iron_Injector.Models;
using Iron_Injector.Models.Abstracts;

using static Iron_Injector.Models.Data;

namespace Iron_Injector.UI
{
    public class ui
    {
        public static void Banner()
        {
            Console.WriteLine
                (
               "██╗██████╗  ██████╗ ███╗   ██╗    ██╗███╗   ██╗     ██╗███████╗ ██████╗████████╗ ██████╗ ██████╗  \n".Pastel(Color.DarkGray) +
               "██║██╔══██╗██╔═══██╗████╗  ██║    ██║████╗  ██║     ██║██╔════╝██╔════╝╚══██╔══╝██╔═══██╗██╔══██╗ \n".Pastel(Color.DarkGray) +
               "██║██████╔╝██║   ██║██╔██╗ ██║    ██║██╔██╗ ██║     ██║█████╗  ██║        ██║   ██║   ██║██████╔╝ \n".Pastel(Color.DarkGray) +
               "██║██╔══██╗██║   ██║██║╚██╗██║    ██║██║╚██╗██║██   ██║██╔══╝  ██║        ██║   ██║   ██║██╔══██ \n".Pastel(Color.DarkGray) +
               "██║██║  ██║╚██████╔╝██║ ╚████║    ██║██║ ╚████║╚█████╔╝███████╗╚██████╗   ██║   ╚██████╔╝██║  ██║ \n".Pastel(Color.DarkGray) +
               "╚═╝╚═╝  ╚═╝ ╚═════╝ ╚═╝  ╚═══╝    ╚═╝╚═╝  ╚═══╝ ╚════╝ ╚══════╝ ╚═════╝   ╚═╝    ╚═════╝ ╚═╝  ╚═╝ \n".Pastel(Color.DarkGray) +
                "\n" +
               "Author: Jon @QueenCityCyber \n".Pastel(Color.DeepSkyBlue) +
               "https://github.com/Queen-City-Cyber \n".Pastel(Color.Green)
                );
        }

        public static void callCommand(string input)
        {
            string[] args = null;

            if (_Commands.Count == 0)
                {loadCommandList();}
            if (_Utilities.Count == 0)
                { loadUtilitiesList(); }
            if (_Modules.Count ==0)
                { loadModuleList(); }

            if (input == "")
                {throw new Exception();}

            Command userCommand = _Commands.FirstOrDefault(c => c.Name.Equals(input.Split(' ')[0], StringComparison.OrdinalIgnoreCase ));
            Utilities userUtilities = _Utilities.FirstOrDefault(u => u.Name.Equals(input.Split(' ')[0], StringComparison.OrdinalIgnoreCase));
            ModuleBase userModules = _Modules.FirstOrDefault(m => m.Name.Equals(input.Split(' ')[0], StringComparison.OrdinalIgnoreCase));

            if (userUtilities == null && userCommand == null && userModules == null)
            {
                Console.WriteLine($"{"[-]".Pastel(Color.Red)}{input} is not an avaible command, please use help for options.");
            }
            if (userCommand != null)
            {
               if (input.Contains(' '))
                    {args = input.Split(' ');}
                
                userCommand.ExecuteCommand(args);
            }

            if (userUtilities != null)
            {

                if (input.Contains(' '))
                { args = input.Split(' '); }

                var results = userUtilities.UtilitiesExecute(args);
                Console.WriteLine(results);    
            }
            if (userModules != null)
            {
                if (input.Contains(' '))
                { args = input.Split(' '); }

                userModules.Init();
            }

        }

        public static void loadCommandList()
        {
            IEnumerable<Command> exporters = typeof(Command)
            .Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(Command)) && !t.IsAbstract)
            .Select(t => (Command)Activator.CreateInstance(t));

            _Commands.AddRange(exporters);
        }

        public static void loadUtilitiesList()
        {
            IEnumerable<Utilities> exporters = typeof(Utilities)
           .Assembly.GetTypes()
           .Where(t => t.IsSubclassOf(typeof(Utilities)) && !t.IsAbstract)
           .Select(t => (Utilities)Activator.CreateInstance(t));

            _Utilities.AddRange(exporters);
        }

        public static void loadModuleList()
        {
            IEnumerable<ModuleBase> exporters = typeof(ModuleBase)
            .Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(ModuleBase)) && !t.IsAbstract)
            .Select(t => (ModuleBase)Activator.CreateInstance(t));

            _Modules.AddRange(exporters);
        }
    }
}
