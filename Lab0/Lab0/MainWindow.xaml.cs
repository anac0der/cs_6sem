using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lab0_ClassLibrary;

namespace Lab0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BM_Click(object sender, RoutedEventArgs e)
        {
            string str = TB_Data.Text;
            DateTime date = DateTime.Now;
            V1DataList obj = new V1DataList(str, date);
            FuncEnum function;
            if (CB.Text == "Field")
            {
                function = FuncEnum.Field;
            }
            else
            {
                function = FuncEnum.Const;
            }
            if (TB_Amount.Text == "")
            {
                return;
            }
            obj.AddDefaults(0, 1, Convert.ToInt32(TB_Amount.Text), function);
            TBOutput.Text = $"ObjectID: {str}, date: {date}\n";
            LB.Items.Clear();
            for (int i = 0; i < obj.Data.Count; ++i)
            {
                LB.Items.Add($"x = {obj.Data[i].x.ToString("F3")} value = {obj.Data[i].value.ToString("F3")}\n");
            }
        }
    }
}
