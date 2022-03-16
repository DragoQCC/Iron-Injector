using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alba.CsConsoleFormat;
using Iron_Injector.Models.Abstracts;
using Iron_Injector.Models;
using Iron_Injector.Commands.Shell_Commands;

namespace Iron_Injector.Commands.Utils
{
    public class Options : Command
    {
        public override string Name => "Options";

        public override string Description => "this shows options table for active module.";

        public override void ExecuteCommand(string[] args)
        {
            if (Program.prompt != Program.OrginalPrompt)
            {
                ModuleBase.Table.MakeTable();
            }
            else
            { Console.WriteLine("Before running Options please use one of the aviliable modules."); }
        }
    }
}
