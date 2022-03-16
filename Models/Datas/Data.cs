using Iron_Injector.Models.Abstracts;
using System.Collections.Generic;


namespace Iron_Injector.Models
{
    internal class Data
    {
        public static readonly List<Command> _Commands = new List<Command>();
        public static readonly List<Utilities> _Utilities = new List<Utilities>();
        public static readonly List<ModuleBase> _Modules = new List<ModuleBase>();
    }
}
