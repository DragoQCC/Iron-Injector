using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iron_Injector.Models.Abstracts;

namespace Iron_Injector.Commands.Utils
{
    public class Clear : Utilities
    {
        public override string Name => "Clear";

        public override string Description => "Clears the console";

        public override string UtilitiesExecute(string[] args)
        {
            string nothing = null;
            Console.Clear();
            return nothing;
        }
    }
}
