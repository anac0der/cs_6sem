using DataLibrary;
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

namespace Lab1_UI_Comments
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewData viewData = new ViewData();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = viewData;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(viewData.ToString());
        }

        private void Button_Click_DoSplines(object sender, RoutedEventArgs e)
        {
            viewData.ExecuteSplines();
            lb_rawData.Items.Clear();
            for(int i = 0; i < viewData.nRawNodes; ++i)
            {
                lb_rawData.Items.Add($"x = {viewData.rawData.rawNodes[i].ToString("F3")} value = {viewData.rawData.rawValues[i].ToString("F3")}");
            }
            lb_splinesData.ItemsSource = viewData.splinesData.values;
            integral_tb.Text = viewData.splinesData.integral.ToString("F3");
        }

        private void lb_splinesData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lb_splinesData.SelectedItem != null)
            {
                selected_tb.Text = ((SplineDataItem)lb_splinesData.SelectedItem).ToString("F3");
            }     
        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            Nullable<bool> result = dlg.ShowDialog();

            string filename = null;
            if (result == true)
            {
                filename = dlg.FileName;
            }
            try
            {
                viewData.Save(filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_Load(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();

            string filename = null;
            if (result == true)
            {
                filename = dlg.FileName;
            }
            try
            {
                viewData.Load(filename);
                this.DataContext = null;
                this.DataContext = viewData;
                Button_Click_DoSplines(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
