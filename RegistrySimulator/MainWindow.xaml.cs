using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RegistrySimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            // Initialize the window and refresh the values
            refreshValues();
        }

        private void InsertClick(object sender, RoutedEventArgs e)
        {
            // Handle the Insert button click
            string registerInsertName = insertComboBox.SelectedItem.ToString().Substring(0, 2);
            string registerValue = insertTextBox.Text.Replace(" ", "");

            if (registerValue == "" || registerValue == null)
            {
                // Show a message if no value is provided
                MessageBox.Show("No value provided.");
                insertTextBox.Text = "";
                return;
            }
            else if (RegisterSimulator.SetRegistry(registerInsertName, registerValue))
            {
                // Refresh values if setting the registry is successful
                MessageBox.Show($"Wprowadzono wartość {registerValue} do rejestru {registerInsertName}.");
                refreshValues();
            }
            else
            {
                // Show a message if the provided value is not a hexadecimal number
                MessageBox.Show("The provided value is not a hexadecimal number.");
                insertTextBox.Text = "";
                return;
            }
        }

        private void RandomClick(object sender, RoutedEventArgs e)
        {
            // Handle the Random button click

            // MessageBox to accept operation
            MessageBoxResult result = MessageBox.Show(
                "Czy na pewno chcesz ustawić na rejestry losowe dane?",
                "Losowe dane",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                return;
            }

            //registers name to foreach
            string[] registerNames = { "AX", "BX", "CX", "DX", "BP", "DI", "SI", "Of"};

            Random random = new Random();

            foreach(string registerName in registerNames)
            {
                //Getting random value to register
                int randomValue = random.Next(1, 101);

                //Setting register
                RegisterSimulator.SetRegistry(registerName, randomValue);
            }

            MessageBox.Show($"Operacja została wykonana.");
            refreshValues();

        }

        private void MovClick(object sender, RoutedEventArgs e)
        {
            // Handle the Mov button click
            string firstRegisterInsertName = movLComboBox.SelectedItem.ToString().Substring(0, 2);
            string secondRegisterInserName = movRComboBox.SelectedItem.ToString().Substring(0, 2);

            int firstRegisterValue = RegisterSimulator.GetRegistryValueFromString(firstRegisterInsertName);
            int secondRegisterValue = RegisterSimulator.GetRegistryValueFromString(secondRegisterInserName);

            if (RegisterSimulator.SetRegistry(secondRegisterInserName, firstRegisterValue))
            {
                // Refresh values if setting the registry is successful
                MessageBox.Show($"Na rejestry {firstRegisterInsertName} ({firstRegisterValue.ToString("X")})" +
                    $" oraz {secondRegisterInserName} ({secondRegisterValue.ToString("X")}) zostałą wykonana operacja MOV.");
                refreshValues();
            }
        }

        private void XchgClick(object sender, RoutedEventArgs e)
        {
            // Handle the Xchg button click
            string firstRegisterInsertName = xchgLComboBox.SelectedItem.ToString().Substring(0, 2);
            string secondRegisterInserName = xchgRComboBox.SelectedItem.ToString().Substring(0, 2);

            int firstRegisterValue = RegisterSimulator.GetRegistryValueFromString(firstRegisterInsertName);
            int secondRegisterValue = RegisterSimulator.GetRegistryValueFromString(secondRegisterInserName);

            if (RegisterSimulator.SetRegistry(firstRegisterInsertName, secondRegisterValue) &&
                RegisterSimulator.SetRegistry(secondRegisterInserName, firstRegisterValue))
            {
                // Refresh values if setting the registry is successful
                MessageBox.Show($"Na rejestry {firstRegisterInsertName} ({firstRegisterValue.ToString("X")})" +
                    $" oraz {secondRegisterInserName} ({secondRegisterValue.ToString("X")}) zostałą wykonana operacja XCHG.");
                refreshValues();
            }
        }

        //to comment \/
        private void movRamLClick(object sender, RoutedEventArgs e)
        {
            refreshMemoryAddress();
        }

        private void refreshMemoryAddress()
        {
            string baseRegister = movRamLComboBox.Text;
            string indexRegister = movRamRComboBox.Text;
            int addressingType = movRamBComboBox.SelectedIndex;

            string memoryAddress = MemorySimulator.getAddressByType(addressingType, baseRegister, indexRegister);

            MessageBox.Show(memoryAddress);
            movRamTextBlock.Text = memoryAddress;
        }

        //to comment /\

        private void refreshValues()
        {
            //-- DISPLAY SECTION --
            // Refresh the displayed values in the window
            axTextBlock.Text = "AX: " + RegisterSimulator.AX.ToString("X");
            bxTextBlock.Text = "BX: " + RegisterSimulator.BX.ToString("X");
            cxTextBlock.Text = "CX: " + RegisterSimulator.CX.ToString("X");
            dxTextBlock.Text = "DX: " + RegisterSimulator.DX.ToString("X");
            bpTextBlock.Text = "BP: " + RegisterSimulator.BP.ToString("X");
            diTextBlock.Text = "DI: " + RegisterSimulator.DI.ToString("X");
            siTextBlock.Text = "SI: " + RegisterSimulator.SI.ToString("X");
            osTextBlock.Text = "Offset: " + RegisterSimulator.Of.ToString("X");

            // Preserve selected index or default to 0 if not selected
            int insertComboBoxSelectedIndex = insertComboBox.SelectedIndex == -1 ? 0 : insertComboBox.SelectedIndex;



            //-- INSERT DATA SECTION --
            // Clear and populate the Insert ComboBox
            insertComboBox.Items.Clear();
            insertComboBox.Items.Add($"AX ({RegisterSimulator.AX.ToString("X")})");
            insertComboBox.Items.Add($"BX ({RegisterSimulator.BX.ToString("X")})");
            insertComboBox.Items.Add($"CX ({RegisterSimulator.CX.ToString("X")})");
            insertComboBox.Items.Add($"DX ({RegisterSimulator.DX.ToString("X")})");
            insertComboBox.Items.Add($"BP ({RegisterSimulator.BP.ToString("X")})");
            insertComboBox.Items.Add($"DI ({RegisterSimulator.DI.ToString("X")})");
            insertComboBox.Items.Add($"SI ({RegisterSimulator.SI.ToString("X")})");
            insertComboBox.Items.Add($"Offset ({RegisterSimulator.Of.ToString("X")})");
            insertComboBox.SelectedIndex = insertComboBoxSelectedIndex;

            // Preserve selected index or default to 0 if not selected
            int movLComboBoxSelectedIndex = movLComboBox.SelectedIndex == -1 ? 0 : movLComboBox.SelectedIndex;



            //-- EDITING DATA SECTION --

            //- tab 1 (MOV)-
            // Clear and populate the MovL ComboBox
            movLComboBox.Items.Clear();
            movLComboBox.Items.Add($"AX ({RegisterSimulator.AX.ToString("X")})");
            movLComboBox.Items.Add($"BX ({RegisterSimulator.BX.ToString("X")})");
            movLComboBox.Items.Add($"CX ({RegisterSimulator.CX.ToString("X")})");
            movLComboBox.Items.Add($"DX ({RegisterSimulator.DX.ToString("X")})");
            movLComboBox.Items.Add($"BP ({RegisterSimulator.BP.ToString("X")})");
            movLComboBox.Items.Add($"DI ({RegisterSimulator.DI.ToString("X")})");
            movLComboBox.Items.Add($"SI ({RegisterSimulator.SI.ToString("X")})");
            movLComboBox.Items.Add($"Offset ({RegisterSimulator.Of.ToString("X")})");
            movLComboBox.SelectedIndex = movLComboBoxSelectedIndex;

            // Preserve selected index or default to 0 if not selected
            int movRComboBoxSelectedIndex = movRComboBox.SelectedIndex == -1 ? 0 : movRComboBox.SelectedIndex;

            // Clear and populate the MovR ComboBox
            movRComboBox.Items.Clear();
            movRComboBox.Items.Add($"AX ({RegisterSimulator.AX.ToString("X")})");
            movRComboBox.Items.Add($"BX ({RegisterSimulator.BX.ToString("X")})");
            movRComboBox.Items.Add($"CX ({RegisterSimulator.CX.ToString("X")})");
            movRComboBox.Items.Add($"DX ({RegisterSimulator.DX.ToString("X")})");
            movRComboBox.Items.Add($"BP ({RegisterSimulator.BP.ToString("X")})");
            movRComboBox.Items.Add($"DI ({RegisterSimulator.DI.ToString("X")})");
            movRComboBox.Items.Add($"SI ({RegisterSimulator.SI.ToString("X")})");
            movRComboBox.Items.Add($"Offset ({RegisterSimulator.Of.ToString("X")})");
            movRComboBox.SelectedIndex = movRComboBoxSelectedIndex;

            // Preserve selected index or default to 0 if not selected
            int xchgLComboBoxSelectedIndex = xchgLComboBox.SelectedIndex == -1 ? 0 : xchgLComboBox.SelectedIndex;


            //- tab 2 (XCHG)-
            // Clear and populate the xchgL ComboBox
            xchgLComboBox.Items.Clear();
            xchgLComboBox.Items.Add($"AX ({RegisterSimulator.AX.ToString("X")})");
            xchgLComboBox.Items.Add($"BX ({RegisterSimulator.BX.ToString("X")})");
            xchgLComboBox.Items.Add($"CX ({RegisterSimulator.CX.ToString("X")})");
            xchgLComboBox.Items.Add($"DX ({RegisterSimulator.DX.ToString("X")})");
            xchgLComboBox.Items.Add($"BP ({RegisterSimulator.BP.ToString("X")})");
            xchgLComboBox.Items.Add($"DI ({RegisterSimulator.DI.ToString("X")})");
            xchgLComboBox.Items.Add($"SI ({RegisterSimulator.SI.ToString("X")})");
            xchgLComboBox.Items.Add($"Offset ({RegisterSimulator.Of.ToString("X")})");
            xchgLComboBox.SelectedIndex = xchgLComboBoxSelectedIndex;

            // Preserve selected index or default to 0 if not selected
            int xchgRComboBoxSelectedIndex = xchgRComboBox.SelectedIndex == -1 ? 0 : xchgRComboBox.SelectedIndex;

            // Clear and populate the xchgR ComboBox
            xchgRComboBox.Items.Clear();
            xchgRComboBox.Items.Add($"AX ({RegisterSimulator.AX.ToString("X")})");
            xchgRComboBox.Items.Add($"BX ({RegisterSimulator.BX.ToString("X")})");
            xchgRComboBox.Items.Add($"CX ({RegisterSimulator.CX.ToString("X")})");
            xchgRComboBox.Items.Add($"DX ({RegisterSimulator.DX.ToString("X")})");
            xchgRComboBox.Items.Add($"BP ({RegisterSimulator.BP.ToString("X")})");
            xchgRComboBox.Items.Add($"DI ({RegisterSimulator.DI.ToString("X")})");
            xchgRComboBox.Items.Add($"SI ({RegisterSimulator.SI.ToString("X")})");
            xchgRComboBox.Items.Add($"Offset ({RegisterSimulator.Of.ToString("X")})");
            xchgRComboBox.SelectedIndex = xchgRComboBoxSelectedIndex;


            //- tab 3 (MOV R->R)-
            // Preserve selected index or default to 0 if not selected
            int movRamLComboBoxSelectedIndex = movRamLComboBox.SelectedIndex == -1 ? 0 : movRamLComboBox.SelectedIndex;

            // Clear and populate the movRamL ComboBox
            movRamLComboBox.Items.Clear();
            movRamLComboBox.Items.Add($"BX ({RegisterSimulator.BX.ToString("X")})");
            movRamLComboBox.Items.Add($"BP ({RegisterSimulator.BP.ToString("X")})");
            movRamLComboBox.SelectedIndex = movRamLComboBoxSelectedIndex;

            // Preserve selected index or default to 0 if not selected
            int movRamRComboBoxSelectedIndex = movRamRComboBox.SelectedIndex == -1 ? 0 :movRamRComboBox.SelectedIndex;

            // Clear and populate the movRamL ComboBox
            movRamRComboBox.Items.Clear();
            movRamRComboBox.Items.Add($"SI ({RegisterSimulator.SI.ToString("X")})");
            movRamRComboBox.Items.Add($"DI ({RegisterSimulator.DI.ToString("X")})");
            movRamRComboBox.SelectedIndex = movRamRComboBoxSelectedIndex;


            // Preserve selected index or default to 0 if not selected
            int movRamBComboBoxSelectedIndex = movRamBComboBox.SelectedIndex == -1 ? 0 : movRamBComboBox.SelectedIndex;

            //Addressing index type combobox
            movRamBComboBox.Items.Clear();
            movRamBComboBox.Items.Add("bazowe");
            movRamBComboBox.Items.Add("indeksowe");
            movRamBComboBox.Items.Add("indeksowo-bazowe");
            movRamBComboBox.SelectedIndex = movRamBComboBoxSelectedIndex;

            //TextBlock
            //movRamTextBlock.Text = "Brak";
        }

    }
}