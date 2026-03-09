using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR12.Entities
{
    internal class ChestItem
    {
        public string Name { get; private set; }
        public string Type { get; private set; }
        public int Value { get; private set; }

        public ChestItem(string name, string type, int value)
        {
            Name = name;
            Type = type;
            Value = value;
        }
    }
}
