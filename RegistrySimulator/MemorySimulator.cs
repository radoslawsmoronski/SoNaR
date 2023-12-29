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


        static public string getAddressByType(int type, string baseRegister, string indexRegister)
        {
            switch (type)
            {
                // Calculate the address by Base Calculation
                case 0:
                    return (RegisterSimulator.GetRegistryValueFromString(baseRegister) +
                            RegisterSimulator.Of).ToString("X");

                // Calculate the address by Index Calculation
                case 1:
                    return (RegisterSimulator.GetRegistryValueFromString(indexRegister) +
                            RegisterSimulator.Of).ToString("X");

                // Calculate the address by Base-Index Calculation
                case 2:
                    return (RegisterSimulator.GetRegistryValueFromString(baseRegister) +
                            RegisterSimulator.GetRegistryValueFromString(indexRegister) +
                            RegisterSimulator.Of).ToString("X");
                default:
                    return null;
            }
        }
    }
}
