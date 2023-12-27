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
        static private Dictionary<string, int> memory = new Dictionary<string, int>();

        // Sets the value at the specified memory address.
        static public void SetValue(string address, int value)
        {
            memory[address] = value;
        }

        //Gets the value stored at the specified memory address.
        static public int GetValue(string address)
        {
            return memory.ContainsKey(address) ? memory[address] : 0; 
        }

        //not working \/

        static public string getAddressByType(int type, string baseRegister, string indexRegister)
        {
            switch (type)
            {
                case 1: return (RegisterSimulator.GetRegistryValueFromString(baseRegister) +
                        RegisterSimulator.Of).ToString("X");
                case 2: return (RegisterSimulator.GetRegistryValueFromString(indexRegister) +
                        RegisterSimulator.Of).ToString("X");
                case 3: return (RegisterSimulator.GetRegistryValueFromString(baseRegister) + 
                        RegisterSimulator.GetRegistryValueFromString(indexRegister) +
                        RegisterSimulator.Of).ToString("X");
                default: return null;
            }
        }
    }
}
