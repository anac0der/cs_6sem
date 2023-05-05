using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary
{
    public class SplinesData
    {
        public int nGrid { get; set; }
        public RawData rawData { get; set; }
        public double leftDer { get; set; }
        public double rightDer { get; set; }

        public List<SplineDataItem> values { get; set; }
        public double integral { get; set; }
        public SplinesData (RawData rawData, double leftDer, double rightDer, int nGrid)
        {
            this.rawData = rawData;
            this.nGrid = nGrid;
            this.leftDer = leftDer;
            this.rightDer = rightDer;
            this.values = new List<SplineDataItem> ();
        }
        public void DoSplines()
        {
            double[] result = new double[3 * nGrid + 1];
            int status = do_splines(rawData.leftEnd, rawData.rightEnd, rawData.rawNodes.Length, rawData.rawNodes, rawData.rawValues, leftDer, rightDer, nGrid, result);
            if (status == 0)
            {
                for (int i = 0; i <= nGrid - 1; ++i)
                {
                    values.Add(new SplineDataItem(rawData.leftEnd + i * ((rawData.rightEnd - rawData.leftEnd) / (nGrid - 1)), result[3 * i], result[3 * i + 1], result[3 * i + 2]));
                }
                integral = result[3 * nGrid];
            }
            else
            {
                throw new Exception("Неудачная интерполяция сплайнами!");
            }
        }

        [DllImport(@"Dll_Library.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int do_splines(double a, double b, int length,double[] nodes, double[] values, double left_der, double right_der, int new_length, double[] res_arr);
    }

    public struct SplineDataItem
    { 
        public double x { get; set;}
        public double[] values { get; set; }
        public SplineDataItem(double x, double zero_der, double first_der, double second_der)
        {
            this.x = x;
            this.values = new double[3];
            values[0] = zero_der;
            values[1] = first_der;
            values[2] = second_der;
        }

        public override string ToString()
        {
            return $"x = {this.x.ToString()}, values = {this.values[0].ToString()}, {this.values[1].ToString()}, {this.values[2].ToString()}";
        }

        public string ToString(string format)
        {
            return $"x = {this.x.ToString(format)}, value = {this.values[0].ToString(format)}, 1st der = {this.values[1].ToString(format)}, 2nd der = {this.values[2].ToString(format)}";
        }

        public string ToLongString(string format)
        {
            return $"x = {this.x.ToString(format)}, value = {this.values[0].ToString(format)}, 1st der = {this.values[1].ToString(format)}, 2nd der = {this.values[2].ToString(format)}";
        }
    }
}
