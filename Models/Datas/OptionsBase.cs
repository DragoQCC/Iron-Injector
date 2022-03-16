using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iron_Injector.Models
{
    public class OptionsBase
    {
        public string Name { get; set; }
        public string CurrentValue { get; set; }
        public bool IsRequired { get; set; }
        public string Description { get; set; }

        public OptionsBase(string name, string currentValue, bool isRequired, string description)
        {
            Name = name;
            CurrentValue = currentValue;
            IsRequired = isRequired;
            Description = description;
        }

    }
}
