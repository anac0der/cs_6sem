using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Legends;

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
    internal class ViewData : IDataErrorInfo
    {
        public string Error {get { return "Error Text"; }}
        public string this[string property]
        {
            get
            {
                string msg = null;
                switch(property)
                {
                    case "nGrid":
                        if (nGrid < 2) msg = "nGrid must be >= 2!";
                        break;
                    case "leftEnd":
                        if (leftEnd >= rightEnd) msg = "leftEnd must be < rightEnd!";
                        break;
                    case "nRawNodes":
                        if (nRawNodes < 2) msg = "nRawNodes must be >= 2!";
                        break;
                    default:
                        break;
                }
                return msg;
            }
        }
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
        public PlotModel? plotModel { get; private set; }
        public bool isCorrect { get; set; }
        public ViewData()
        {
            leftEnd = 0;
            rightEnd = 1;
            nRawNodes = 5;
            nGrid = 10;
            isUniform = true;
            plotModel = new PlotModel { Title = "График получившегося сплайна" };
            listFRaw = new List<FRaw>();       // для способа 2
            listFRaw.Add(RawData.Linear);      // для способа 2
            listFRaw.Add(RawData.Random);   // для способа 2
            listFRaw.Add(RawData.Cubic); // для способа 2
            fRaw = listFRaw[1];                // для способа 2
            rawData = new RawData(leftEnd, rightEnd, nRawNodes, isUniform, fRaw);
        }
        public void ExecuteSplines()
        {
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

        public void UpdateGraph()
        {
            plotModel?.Series.Clear();
            OxyColor color = OxyColors.Green;
            LineSeries lineSeries = new LineSeries();

            foreach (SplineDataItem value in splinesData.values)
            {
                lineSeries.Points.Add(new DataPoint(value.x, value.values[0]));
            };
            lineSeries.Color = color;
            lineSeries.Title = "Сплайн";
            lineSeries.MarkerType = MarkerType.Square;
            lineSeries.MarkerStroke = color;
            lineSeries.MarkerFill = color;

            Legend legend = new Legend();
            plotModel?.Legends.Add(legend);
            plotModel?.Series.Add(lineSeries);

            color = OxyColors.Red;
            lineSeries = new LineSeries();

            for (int i = 0; i < rawData.rawNodes.Length; ++i)
            {
                lineSeries.Points.Add(new DataPoint(rawData.rawNodes[i], rawData.rawValues[i]));
            };
            lineSeries.Color = OxyColors.Transparent;
            lineSeries.Title = "Дискретная сетка";
            lineSeries.MarkerType = MarkerType.Square;
            lineSeries.MarkerStroke = color;
            lineSeries.MarkerFill = color;
            plotModel?.Series.Add(lineSeries);
            plotModel?.InvalidatePlot(true);
        }
        public void Save(string filename)
        {
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
            rawData = newRawData;
        }
    }
}
