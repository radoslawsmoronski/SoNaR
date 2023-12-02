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

        int ax, bx, cx, dx;

        public MainWindow()
        {
            InitializeComponent();

            insertElementsToComboBoxes();

        }

        private void insertElementsToComboBoxes()
        {
            insertComboBox.Items.Add("AX");
            insertComboBox.Items.Add("BX");
            insertComboBox.Items.Add("CX");
            insertComboBox.Items.Add("DX");

            movLComboBox.Items.Add("AX");
            movLComboBox.Items.Add("BX");
            movLComboBox.Items.Add("CX");
            movLComboBox.Items.Add("DX");

            movRComboBox.Items.Add("AX");
            movRComboBox.Items.Add("BX");
            movRComboBox.Items.Add("CX");
            movRComboBox.Items.Add("DX");
        }

        private void InsertClick(object sender, RoutedEventArgs e)
        {
            var selectedObject = insertComboBox.SelectedItem;
            string insertValue = insertTextBox.Text.Replace(" ", "");

            if (insertValue == "" || insertValue == null)
            {
                MessageBox.Show("Nie podano żadnej wartości.");
                insertTextBox.Text = "";
                return;
            }
            else if (IsHexadecimal(insertValue, out int  value))
            {
                switch (selectedObject)
                {
                    case "AX":
                        ax = value;
                        axTextBlock.Text = "AX: " + ax.ToString("X");
                        break;
                    case "BX":
                        bx = value;
                        bxTextBlock.Text = "BX: " + bx.ToString("X");
                        break;
                    case "CX":
                        cx = value;
                        cxTextBlock.Text = "CX: " + cx.ToString("X");
                        break;
                    case "DX":
                        dx = value;
                        dxTextBlock.Text = "DX: " + dx.ToString("X");
                        break;
                }
            }
            else
            {
                MessageBox.Show("Podana wartość nie jest liczbą heksadecy.");
                insertTextBox.Text = "";
                return;
            }
        }

        static bool IsHexadecimal(string input, out int value)
        {
            return int.TryParse(input, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value);
        }

    }
}
