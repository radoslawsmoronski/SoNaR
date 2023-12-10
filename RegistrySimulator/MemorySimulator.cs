using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RegistrySimulator
{
    public class MemorySimulator
    {
        // Dictionary to store memory values.
        private Dictionary<string, int> memory = new Dictionary<string, int>();

        // Sets the value at the specified memory address.
        public void SetValue(string address, int value)
        {
            memory[address] = value;
        }

        //Gets the value stored at the specified memory address.
        public int GetValue(string address)
        {
            return memory.ContainsKey(address) ? memory[address] : 0; 
        }
    }
}
