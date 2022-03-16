using Iron_Injector.Models.Abstracts;
using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iron_Injector.Commands.Utils
{
    public class help : Utilities
    {
        public override string Name => "help";

        public override string Description => "Prints help menu and lists commands.";

        public override string UtilitiesExecute(string[] args)
        {
            StringBuilder _output = new StringBuilder();
            _output.AppendLine($"{"[+]".Pastel(Color.Green)}This is the help menu for now.");
            _output.AppendLine($"{"[+]".Pastel(Color.Green)}Universial Commands: (help, clear, exit)");
            _output.AppendLine($"{"[+]".Pastel(Color.Green)}Modules: (ShellLoader, programLoader)");
            _output.AppendLine($"{"[+]".Pastel(Color.Green)}Module Commands: (Set, Options, Run)");
            _output.AppendLine($"{"[+]".Pastel(Color.Green)}After an Assembly has been loaded, if you exit the interactive prompt reconnect to it with ConnectAssembly assemblyname. ex ConnectAssembly Rubeus.exe");
            _output.AppendLine($"{"[+]".Pastel(Color.Green)} when loading shellcode the aes encryption password is !r0nInj3ct0r123! , check out my aes encryption project for a easy encryption to go with this program.");

            return _output.ToString();
        }
    }
}
