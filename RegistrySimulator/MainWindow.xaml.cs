﻿using Microsoft.Win32;
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
            string selectedObject = insertComboBox.SelectedItem.ToString().Substring(0, 2);
            string insertValue = insertTextBox.Text.Replace(" ", "");

            if (insertValue == "" || insertValue == null)
            {
                // Show a message if no value is provided
                MessageBox.Show("No value provided.");
                insertTextBox.Text = "";
                return;
            }
            else if (RegisterSimulator.SetRegistry(selectedObject.ToString(), insertValue))
            {
                // Refresh values if setting the registry is successful
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

        private void refreshValues()
        {
            // Refresh the displayed values in the window
            axTextBlock.Text = "AX: " + RegisterSimulator.AX.ToString("X");
            bxTextBlock.Text = "BX: " + RegisterSimulator.BX.ToString("X");
            cxTextBlock.Text = "CX: " + RegisterSimulator.CX.ToString("X");
            dxTextBlock.Text = "DX: " + RegisterSimulator.DX.ToString("X");

            // Preserve selected index or default to 0 if not selected
            int insertComboBoxSelectedIndex = insertComboBox.SelectedIndex == -1 ? 0 : insertComboBox.SelectedIndex;

            // Clear and populate the Insert ComboBox
            insertComboBox.Items.Clear();
            insertComboBox.Items.Add($"AX ({RegisterSimulator.AX.ToString("X")})");
            insertComboBox.Items.Add($"BX ({RegisterSimulator.BX.ToString("X")})");
            insertComboBox.Items.Add($"CX ({RegisterSimulator.CX.ToString("X")})");
            insertComboBox.Items.Add($"DX ({RegisterSimulator.DX.ToString("X")})");
            insertComboBox.SelectedIndex = insertComboBoxSelectedIndex;

            // Preserve selected index or default to 0 if not selected
            int movLComboBoxSelectedIndex = movLComboBox.SelectedIndex == -1 ? 0 : movLComboBox.SelectedIndex;

            // Clear and populate the MovL ComboBox
            movLComboBox.Items.Clear();
            movLComboBox.Items.Add($"AX ({RegisterSimulator.AX.ToString("X")})");
            movLComboBox.Items.Add($"BX ({RegisterSimulator.BX.ToString("X")})");
            movLComboBox.Items.Add($"CX ({RegisterSimulator.CX.ToString("X")})");
            movLComboBox.Items.Add($"DX ({RegisterSimulator.DX.ToString("X")})");
            movLComboBox.SelectedIndex = movLComboBoxSelectedIndex;

            // Preserve selected index or default to 0 if not selected
            int movRComboBoxSelectedIndex = movRComboBox.SelectedIndex == -1 ? 0 : movRComboBox.SelectedIndex;

            // Clear and populate the MovR ComboBox
            movRComboBox.Items.Clear();
            movRComboBox.Items.Add($"AX ({RegisterSimulator.AX.ToString("X")})");
            movRComboBox.Items.Add($"BX ({RegisterSimulator.BX.ToString("X")})");
            movRComboBox.Items.Add($"CX ({RegisterSimulator.CX.ToString("X")})");
            movRComboBox.Items.Add($"DX ({RegisterSimulator.DX.ToString("X")})");
            movRComboBox.SelectedIndex = movRComboBoxSelectedIndex;

            // Preserve selected index or default to 0 if not selected
            int xchgLComboBoxSelectedIndex = xchgLComboBox.SelectedIndex == -1 ? 0 : xchgLComboBox.SelectedIndex;

            // Clear and populate the xchgL ComboBox
            xchgLComboBox.Items.Clear();
            xchgLComboBox.Items.Add($"AX ({RegisterSimulator.AX.ToString("X")})");
            xchgLComboBox.Items.Add($"BX ({RegisterSimulator.BX.ToString("X")})");
            xchgLComboBox.Items.Add($"CX ({RegisterSimulator.CX.ToString("X")})");
            xchgLComboBox.Items.Add($"DX ({RegisterSimulator.DX.ToString("X")})");
            xchgLComboBox.SelectedIndex = xchgLComboBoxSelectedIndex;

            // Preserve selected index or default to 0 if not selected
            int xchgRComboBoxSelectedIndex = xchgRComboBox.SelectedIndex == -1 ? 0 : xchgRComboBox.SelectedIndex;

            // Clear and populate the xchgR ComboBox
            xchgRComboBox.Items.Clear();
            xchgRComboBox.Items.Add($"AX ({RegisterSimulator.AX.ToString("X")})");
            xchgRComboBox.Items.Add($"BX ({RegisterSimulator.BX.ToString("X")})");
            xchgRComboBox.Items.Add($"CX ({RegisterSimulator.CX.ToString("X")})");
            xchgRComboBox.Items.Add($"DX ({RegisterSimulator.DX.ToString("X")})");
            xchgRComboBox.SelectedIndex = xchgRComboBoxSelectedIndex;


        }

    }
}