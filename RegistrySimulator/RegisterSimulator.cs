using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace RegistrySimulator
{
    public static class RegisterSimulator
    {
        // Define registers AX, BX, CX, DX
        public static int AX { get; set; }
        public static int BX { get; set; }
        public static int CX { get; set; }
        public static int DX { get; set; }
        public static int BP { get; set; }
        public static int DI { get; set; }
        public static int SI { get; set; }
        public static int Of { get; set; }

        // Method to set registry value based on register name and value
        public static bool SetRegistry(string registerName, object value)
        {
            int registerValue;

            // Convert hexadecimal string to int if the value is a hexadecimal string
            if ((value is string valueString) && (IsHexadecimal(valueString, out int intValue)))
            {
                registerValue = intValue;
            }
            // Use the value directly if it's an integer
            else if (value is int directIntValue)
            {
                registerValue = directIntValue;
            }
            else
            {
                return false; // Unsupported value type
            }

            // Set the value of the specified register
            PropertyInfo property = typeof(RegisterSimulator).GetProperty(registerName);
            if (property != null)
            {
                property.SetValue(null, registerValue);
                return true; // Successfully set registry value
            }
            else
            {
                MessageBox.Show("Error: Unknown register name"); // Register name not found
            }

            return false; // Failed to set registry value
        }

        // Method to get registry value from a specified register name
        public static int GetRegistryValueFromString(string registerName)
        {
            PropertyInfo property = typeof(RegisterSimulator).GetProperty(registerName);

            if (property != null)
            {
                return (int)property.GetValue(null); // Return the value of the specified register
            }
            else
            {
                MessageBox.Show("Error: Unknown register name - " + registerName); // Register name not found
                return 0;
            }
        }

        // Method to check if a string is a valid hexadecimal representation and convert it to an int
        public static bool IsHexadecimal(string input, out int value)
        {
            return int.TryParse(input, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value);
        }
    }
}