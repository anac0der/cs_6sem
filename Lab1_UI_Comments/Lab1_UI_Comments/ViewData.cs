using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary;
using System.Windows;
using System.Windows.Data;

namespace Lab1_UI_Comments
{
    [ValueConversion(typeof(double[]), typeof(string))]
    public class IntervalConverter : IMultiValueConverter
    {
        public object[] ConvertBack(object values, Type[] targetType, object parameter,
    System.Globalization.CultureInfo culture)
        {
            object[] res = new object[2];
            res[0]= Double.Parse(((string)values).Split(' ')[0]);
            res[1] = Double.Parse(((string)values).Split(' ')[1]);
            return res;
        }
        public object Convert(object[] values, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            return $"{values[0]} {values[1]}" as object; 
        }
    }
    internal class ViewData
    {
        public double leftEnd { get; set; }
        public double rightEnd { get; set; }
        public double leftDer { get; set; }
        public double rightDer { get; set; }
        public bool isUniform { get; set; }
        public int nRawNodes { get; set; }
        public int nGrid { get; set; }
        public FRawEnum fRawEnum { get; set; } // Способ 1
        public List<FRaw> listFRaw { get; set; }  // Способ 2. Список делегатов
        public FRaw fRaw { get; set; }  // для способа 2
        public RawData? rawData { get; set; }
        public SplinesData? splinesData { get; set; }
        public ViewData()
        {
            leftEnd = 0;
            rightEnd = 1;
            nRawNodes = 5;
            nGrid = 10;

            listFRaw = new List<FRaw>();       // для способа 2
            listFRaw.Add(RawData.Linear);      // для способа 2
            listFRaw.Add(RawData.Random);   // для способа 2
            listFRaw.Add(RawData.Cubic); // для способа 2
            fRaw = listFRaw[2];                // для способа 2
        }
        public void ExecuteSplines()
        {
            rawData = new RawData(leftEnd, rightEnd, nRawNodes, isUniform, fRaw); 
            splinesData = new SplinesData(rawData, leftDer, rightDer, nGrid);
            splinesData.DoSplines();
        }
        public override string ToString()
        {
            return $"leftEnd = {leftEnd}\n" +
                   $"rightEnd = {rightEnd}\n" +
                   $"nRawNodes = {nRawNodes}\n" +
                   $"isUniform = {isUniform}\n" +
                   $"nGrid = {nGrid}\n" +
                   $"leftDer = {leftDer}\n" +
                   $"rightDer = {rightDer}\n" +
                   $"fRaw = {fRaw.Method.Name}\n"; 
        }

        public void Save(string filename)
        {
            rawData = new RawData(leftEnd, rightEnd, nRawNodes, isUniform, fRaw);
            rawData.Save(filename);
        }

        public void Load(string filename)
        {
            RawData newRawData = new RawData(0, 1, 5, true, RawData.Linear);
            RawData.Load(filename, ref newRawData);
            leftEnd = newRawData.leftEnd;
            rightEnd = newRawData.rightEnd;
            nRawNodes = newRawData.nRawNodes;
            isUniform = newRawData.isUniform;
            fRaw = newRawData.fRaw;
        }
    }
}
