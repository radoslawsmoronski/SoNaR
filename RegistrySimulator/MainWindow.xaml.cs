using System;
using System.Collections.Generic;
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

        }
    }
}
