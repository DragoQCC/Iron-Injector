using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iron_Injector.Models.Abstracts
{
    public abstract class Command
    {
        public abstract string Name { get; }   
        public abstract string Description { get; }

        public abstract void ExecuteCommand(string[] args);
    }
}
