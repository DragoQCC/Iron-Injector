using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Iron_Injector.Models.Abstracts;

namespace Iron_Injector.Commands.Program_Commands
{
    internal class ConnectAssembly : Command
    {
        public override string Name => "ConnectAssembly";

        public override string Description => "Re connects user back into assembly interaction mode if assembly is still present. Enter the exact filename to re interact.";

        public override void ExecuteCommand(string[] args)
        {
            string assemblyName = args[1];
            Assembly currentAssem = ExecuteAssembly.assemList[assemblyName];
            string input;
            while (true)
            {
                Console.Write($"[{assemblyName}]> ");
                input = Console.ReadLine();

                if (input == "exit")
                {
                    break;
                }
                currentAssem.EntryPoint.Invoke(null, new[] { $"{input}".Split() });
            }
        }
    }
}
