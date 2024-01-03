using Microsoft.Win32;
using System;
using System.Collections;
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
            string registerInsertName = insertComboBox.SelectedItem.ToString().Substring(0, 2); //Getting two first characters, because string looks like it "AX (xx)"
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

        private void MovUClick(object sender, RoutedEventArgs e)
        {
            // Handle the Mov upper button click
            string firstRegisterInsertName = movULComboBox.SelectedItem.ToString().Substring(0, 2); //Getting two first characters, because string looks like it "AX (xx)"
            string secondRegisterInserName = movURComboBox.SelectedItem.ToString().Substring(0, 2); //Getting two first characters, because string looks like it "AX (xx)"

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

        private void MovBClick(object sender, RoutedEventArgs e)
        {
            // Handle the Mov bottom button click

            if (!movBLComboBox.IsEnabled)
            {
                MessageBox.Show("Nie można wykonać operacji, ponieważ pamięć operacyjna jest pusta, co powoduje brak potrzebnego elementu do operacji MOV.");
                return;
            }

            string ramAddress = movBLComboBox.SelectedItem.ToString();
                int index = ramAddress.IndexOf(' ');
                ramAddress = index != -1 ? ramAddress.Substring(0, index) : ramAddress;

            string registerName = movBRComboBox.SelectedItem.ToString().Substring(0, 2); //Getting two first characters, because string looks like it "AX (xx)"

            int ramValue = MemorySimulator.GetValue(ramAddress);
            int registerValue = RegisterSimulator.GetRegistryValueFromString(registerName);

            if (RegisterSimulator.SetRegistry(registerName, ramValue))
            {
                // Refresh values if setting the registry is successful
                MessageBox.Show($"Na rejestr {registerName} ({registerValue.ToString("X")})" +
                    $" oraz komórkę w pamięci operacyjnej o adresie {ramAddress} ({ramValue.ToString("X")}) została wykonana operacja MOV.");
                refreshValues();
            }
        }

        private void MovRamSelectionChanged(object sender, RoutedEventArgs e)
        {
            // Handle the movRamCombobox changed selection

            if (isAddingValuesToComboBoxes) // bool to block event MovRamSelectionChanged when Comboboxes are refreshing
                refreshMemoryAddress();
        }

        private void MovRamClick(object sender, RoutedEventArgs e)
        {
            // Handle the MOV 2 button click
            string registerToMov = movRamUComboBox.SelectedItem.ToString().Substring(0, 2); //Getting two first characters, because string looks like it "AX (xx)"
            string baseRegister = movRamLComboBox.SelectedItem.ToString().Substring(0, 2); //Getting two first characters, because string looks like it "AX (xx)"
            string indexRegister = movRamRComboBox.SelectedItem.ToString().Substring(0, 2); //Getting two first characters, because string looks like it "AX (xx)"
            int addressingType = movRamBComboBox.SelectedIndex;

            int registerToMovValue = RegisterSimulator.GetRegistryValueFromString(registerToMov);
            string memoryAddress = MemorySimulator.getAddressByType(addressingType, baseRegister, indexRegister);

            MemorySimulator.SetValue(memoryAddress, registerToMovValue);

            MessageBox.Show($"Na komórce w pamięci operacyjnej o adresie {memoryAddress}" +
                $" wykonano operacje MOV z rejestru {registerToMov}({registerToMovValue.ToString("X")});");

            refreshValues();
        }

        private void XchgUClick(object sender, RoutedEventArgs e)
        {
            // Handle the XCHG button click
            string firstRegisterInsertName = xchgULComboBox.SelectedItem.ToString().Substring(0, 2); //Getting two first characters, because string looks like it "AX (xx)"
            string secondRegisterInserName = xchgURComboBox.SelectedItem.ToString().Substring(0, 2); //Getting two first characters, because string looks like it "AX (xx)"

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

        private void XchgBClick(object sender, RoutedEventArgs e)
        {
            // Handle the xchg bottom button click

            if (!xchgBLComboBox.IsEnabled)
            {
                MessageBox.Show("Nie można wykonać operacji, ponieważ pamięć operacyjna jest pusta, co powoduje brak potrzebnego elementu do operacji MOV.");
                return;
            }

            string ramAddress = xchgBLComboBox.SelectedItem.ToString();
            int index = ramAddress.IndexOf(' ');
            ramAddress = index != -1 ? ramAddress.Substring(0, index) : ramAddress;

            string registerName = xchgBRComboBox.SelectedItem.ToString().Substring(0, 2); //Getting two first characters, because string looks like it "AX (xx)"

            int ramValue = MemorySimulator.GetValue(ramAddress);
            int registerValue = RegisterSimulator.GetRegistryValueFromString(registerName);

            if (RegisterSimulator.SetRegistry(registerName, ramValue))
            {
                MemorySimulator.SetValue(ramAddress, registerValue);
                // Refresh values if setting the registry is successful
                MessageBox.Show($"Na rejestr {registerName} ({registerValue.ToString("X")})" +
                    $" oraz komórkę w pamięci operacyjnej o adresie {ramAddress} ({ramValue.ToString("X")}) została wykonana operacja XCHG.");
                refreshValues();
            }
        }

        private void PushClick(object sender, RoutedEventArgs e)
        {
            string registerName = pushComboBox.SelectedItem.ToString().Substring(0, 2); //Getting two first characters, because string looks like it "AX (xx)"
            RegisterSimulator.Push(registerName);
            MessageBox.Show($"Wykonano operację PUSH z rejestru {registerName}" +
                $"({RegisterSimulator.GetRegistryValueFromString(registerName).ToString("X")})");
            refreshValues();
        }
        private void PopClick(object sender, RoutedEventArgs e)
        {
            string registerName = popComboBox.SelectedItem.ToString().Substring(0, 2); //Getting two first characters, because string looks like it "AX (xx)"
            if(RegisterSimulator.Pop(registerName))
            {
                MessageBox.Show($"Wykonano operację POP do rejestru {registerName}" +
                    $"({RegisterSimulator.GetRegistryValueFromString(registerName).ToString("X")})");
                refreshValues();
            }
            else
            {
                MessageBox.Show("Stos jest pusty.");
            }

        }

        private bool isAddingValuesToComboBoxes = false; // bool to block event MovRamSelectionChanged when Comboboxes are refreshing

        public class RamMemoryView
        {
            public string Column1 { get; set; }
            public string Column2 { get; set; }
        };

        private void refreshValues()
        {
            isAddingValuesToComboBoxes = false; // bool to block event MovRamSelectionChanged when Comboboxes are refreshing
            Dictionary<string, int> memory = MemorySimulator.GetMemoryValues();





            // ###############################
            // ### SECTION 1: DISPLAYING   ###
            // ###############################



                // ------------ SECTION 1: REGISTERS TEXTBLOCKS ------------

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


                // ------------ SECTION 1: RAM LISTVIEW ------------

                ramListView.Items.Clear();
                foreach (KeyValuePair<string, int> entry in memory)
                {
                    string key = entry.Key;
                    int value = entry.Value;

                    RamMemoryView ramValue = new RamMemoryView { Column1 = entry.Key, Column2 = entry.Value.ToString("X") };

                    ramListView.Items.Add(ramValue);
                }

                // ------------ SECTION 1: STACK LISTVIEW ------------

                Stack<int> stack = RegisterSimulator.GetStack();

                stackListView.Items.Clear();
                foreach (int value in stack)
                {
                    stackListView.Items.Add(value.ToString("X"));
                }


                // #######################################
                // ### SECTION 2: INSERT AND EDITING   ###
                // #######################################



                // ------------ SECTION 2: INSERT DATA ------------

                refreshAllRegistryValuesComboBox(insertComboBox);



                //// ------------ SECTION 2: EDITING DATA ------------


                    //// <------- SECTION 2, EDITING DATA: TAB 1 ------->
            
                    refreshAllRegistryValuesComboBox(movULComboBox);
                    refreshAllRegistryValuesComboBox(movURComboBox);

                    // Preserve selected index or default to 0 if not selected
                    int movBLComboBoxSelectedIndex = movBLComboBox.SelectedIndex == -1 ? 0 : movBLComboBox.SelectedIndex;

                    // Clear and populate the movRamL ComboBox
                    movBLComboBox.Items.Clear();

                    if (memory.Count != 0)
                    {
                        foreach (KeyValuePair<string, int> entry in memory)
                        {
                            string key = entry.Key;
                            int value = entry.Value;

                            RamMemoryView ramValue = new RamMemoryView { Column1 = entry.Key, Column2 = entry.Value.ToString("X") };

                            movBLComboBox.Items.Add($"{entry.Key} ({entry.Value.ToString("X")})");
                        }
                        movBLComboBox.IsEnabled = true;
                        movBLComboBox.SelectedIndex = movBLComboBoxSelectedIndex;
                    }
                    else
                    {
                        movBLComboBox.IsEnabled = false;
                    }

                    refreshAllRegistryValuesComboBox(movBRComboBox);


                    //// <------- SECTION 2, EDITING DATA: TAB 2 ------->

                    refreshAllRegistryValuesComboBox(movRamUComboBox);

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

                    refreshMemoryAddress();


                    //// <------- SECTION 2, EDITING DATA: TAB 3 ------->

                    refreshAllRegistryValuesComboBox(xchgULComboBox);
                    refreshAllRegistryValuesComboBox(xchgURComboBox);

                    // Preserve selected index or default to 0 if not selected
                    int xchgBLComboBoxSelectedIndex = xchgBLComboBox.SelectedIndex == -1 ? 0 : xchgBLComboBox.SelectedIndex;

                    // Clear and populate the movRamL ComboBox
                    xchgBLComboBox.Items.Clear();

                    if (memory.Count != 0)
                    {
                        foreach (KeyValuePair<string, int> entry in memory)
                        {
                            string key = entry.Key;
                            int value = entry.Value;

                            RamMemoryView ramValue = new RamMemoryView { Column1 = entry.Key, Column2 = entry.Value.ToString("X") };

                            xchgBLComboBox.Items.Add($"{entry.Key} ({entry.Value.ToString("X")})");
                        }
                        xchgBLComboBox.IsEnabled = true;
                        xchgBLComboBox.SelectedIndex = movBLComboBoxSelectedIndex;
                    }
                    else
                    {
                        xchgBLComboBox.IsEnabled = false;
                    }

                    refreshAllRegistryValuesComboBox(xchgBRComboBox);


                    //// <------- SECTION 2, EDITING DATA: TAB 3 ------->
                    
                    // Preserve selected index or default to 0 if not selected
                    int pushComboBoxSelectedIndex = pushComboBox.SelectedIndex == -1 ? 0 : pushComboBox.SelectedIndex;

                    //Addressing index type combobox
                    pushComboBox.Items.Clear();
                    pushComboBox.Items.Add($"AX ({RegisterSimulator.AX.ToString("X")})");
                    pushComboBox.Items.Add($"BX ({RegisterSimulator.BX.ToString("X")})");
                    pushComboBox.Items.Add($"CX ({RegisterSimulator.CX.ToString("X")})");
                    pushComboBox.Items.Add($"DX ({RegisterSimulator.DX.ToString("X")})");
                    pushComboBox.SelectedIndex = pushComboBoxSelectedIndex;

                    // Preserve selected index or default to 0 if not selected
                    int popComboBoxSelectedIndex = popComboBox.SelectedIndex == -1 ? 0 : popComboBox.SelectedIndex;

                    //Addressing index type combobox
                    popComboBox.Items.Clear();
                    popComboBox.Items.Add($"AX ({RegisterSimulator.AX.ToString("X")})");
                    popComboBox.Items.Add($"BX ({RegisterSimulator.BX.ToString("X")})");
                    popComboBox.Items.Add($"CX ({RegisterSimulator.CX.ToString("X")})");
                    popComboBox.Items.Add($"DX ({RegisterSimulator.DX.ToString("X")})");
                    popComboBox.SelectedIndex = popComboBoxSelectedIndex;


            isAddingValuesToComboBoxes = true; // bool to block event MovRamSelectionChanged when Comboboxes are refreshing
        }

        private void refreshMemoryAddress()
        {
            string baseRegister = movRamLComboBox.SelectedItem.ToString().Substring(0, 2); //Getting two first characters, because string looks like it "AX (xx)"
            string indexRegister = movRamRComboBox.SelectedItem.ToString().Substring(0, 2); //Getting two first characters, because string looks like it "AX (xx)"
            int addressingType = movRamBComboBox.SelectedIndex;

            string memoryAddress = MemorySimulator.getAddressByType(addressingType, baseRegister, indexRegister);

            movRamTextBlock.Text = memoryAddress;
        }

        private void refreshAllRegistryValuesComboBox(ComboBox combo) //Support method to refresh ComboBoxes
        {
            // Get id to set the index on previosly value
            int comboSelectedIndex = combo.SelectedIndex == -1 ? 0 : combo.SelectedIndex;

            // Clear and populate the xchgR ComboBox
            combo.Items.Clear();
            combo.Items.Add($"AX ({RegisterSimulator.AX.ToString("X")})");
            combo.Items.Add($"BX ({RegisterSimulator.BX.ToString("X")})");
            combo.Items.Add($"CX ({RegisterSimulator.CX.ToString("X")})");
            combo.Items.Add($"DX ({RegisterSimulator.DX.ToString("X")})");
            combo.Items.Add($"BP ({RegisterSimulator.BP.ToString("X")})");
            combo.Items.Add($"DI ({RegisterSimulator.DI.ToString("X")})");
            combo.Items.Add($"SI ({RegisterSimulator.SI.ToString("X")})");
            combo.Items.Add($"Offset ({RegisterSimulator.Of.ToString("X")})");
            combo.SelectedIndex = comboSelectedIndex;
        }

    }
}