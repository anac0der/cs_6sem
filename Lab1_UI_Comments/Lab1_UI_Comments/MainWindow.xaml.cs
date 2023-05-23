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
            try
            {
                for (int i = 0; i < viewData.nRawNodes; ++i)
                {
                    lb_rawData.Items.Add($"x = {viewData.rawData.rawNodes[i].ToString("F3")} value = {viewData.rawData.rawValues[i].ToString("F3")}");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Данные изменились, сначала нажмите кнопку RawData from source!");
            }   
            lb_splinesData.ItemsSource = viewData.splinesData.values;
            integral_tb.Text = viewData.splinesData.integral.ToString("F3");
            try
            {
                viewData.UpdateGraph();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.DataContext = viewData;
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
                lb_rawData.Items.Clear();
                for (int i = 0; i < viewData.nRawNodes; ++i)
                {
                    lb_rawData.Items.Add($"x = {viewData.rawData.rawNodes[i].ToString("F3")} value = {viewData.rawData.rawValues[i].ToString("F3")}");
                }
                this.DataContext = null;
                this.DataContext = viewData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void RightSplineData(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                string temp;
                bool can_execute = true;

                temp = viewData["leftEnd"];
                can_execute = temp == null ? can_execute : false;

                temp = viewData["nGrid"];
                can_execute = temp == null ? can_execute : false;

                if (viewData.rawData == null)
                {
                    can_execute = false;
                }

                if (can_execute)
                    e.CanExecute = true;
                else
                    e.CanExecute = false;
            }
            catch (Exception ex)
            {
                e.CanExecute = false;
            }
        }
        public void RightInfo(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                string temp;
                bool can_execute = true;

                temp = viewData["leftEnd"];
                can_execute = temp == null ? can_execute : false;

                temp = viewData["nGrid"];
                can_execute = temp == null ? can_execute : false;

                if (can_execute)
                    e.CanExecute = true;
                else e.CanExecute = false;
            }
            catch (Exception ex)
            {
                e.CanExecute = false;
            }
        }
        public void RightInput(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                string temp;
                bool can_execute = true;

                temp = viewData["leftEnd"];
                can_execute = temp == null ? can_execute : false;

                temp = viewData["nRawNodes"];
                can_execute = temp == null ? can_execute : false;

                temp = viewData["nGrid"];
                can_execute = temp == null ? can_execute : false;

                if (can_execute)
                    e.CanExecute = true;
                else
                    e.CanExecute = false;
            }
            catch (Exception ex)
            {
                e.CanExecute = false;
            }
        }
        public void RawDataControls(object sender, ExecutedRoutedEventArgs e)
        {
            viewData.rawData = new RawData(viewData.leftEnd, viewData.rightEnd,
                    viewData.nRawNodes, viewData.isUniform, viewData.fRaw);
            lb_rawData.Items.Clear();
            for (int i = 0; i < viewData.nRawNodes; ++i)
            {
                lb_rawData.Items.Add($"x = {viewData.rawData.rawNodes[i].ToString("F3")} value = {viewData.rawData.rawValues[i].ToString("F3")}");
            }
            DataContext = null;
            DataContext = viewData;
        }
        public void RawData_IsNull(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                if (viewData.rawData == null)
                {
                    e.CanExecute = false;
                }
                else
                {
                    e.CanExecute = true;
                }
            }
            catch (Exception ex)
            {
                e.CanExecute = false;
            }
        }
    }
}
