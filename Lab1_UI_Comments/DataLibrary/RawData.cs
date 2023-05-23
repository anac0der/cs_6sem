using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataLibrary
{
    public delegate double FRaw(double x);
    public enum FRawEnum
    {
        Linear, Random, Cubic
    }
    public class RawData
    {
        public double leftEnd { get; set; }
        public double rightEnd { get; set; }
        public int nRawNodes { get; set; }
        public bool isUniform { get; set; }
        public double[] rawNodes { get; set; }
        public double[] rawValues { get; set; }
        public FRaw fRaw { get; set; }
        public RawData (double leftEnd, double rightEnd, int nRawNodes, bool isUniform, FRaw fRaw)
        {
            this.leftEnd = leftEnd;
            this.rightEnd = rightEnd;
            this.isUniform = isUniform;
            this.nRawNodes = nRawNodes;
            this.rawNodes = new double[nRawNodes];
            this.fRaw = fRaw;
            if (isUniform)
            {
                for(int i = 0; i <= nRawNodes - 1; ++i)
                {
                    rawNodes[i] = leftEnd + i * ((rightEnd - leftEnd) / (nRawNodes - 1));
                }
            }
            else
            {
                Random rng = new Random(1);
                for (int i = 0; i < nRawNodes - 2; ++i)
                {
                    rawNodes[i] = leftEnd + (rightEnd - leftEnd) * rng.NextDouble();
                }
                rawNodes[nRawNodes - 2] = leftEnd;
                rawNodes[nRawNodes - 1] = rightEnd; 
                Array.Sort(rawNodes);
            }
            this.rawValues = new double[nRawNodes];
            for (int i = 0; i < nRawNodes; ++i)
            {
                rawValues[i] = fRaw(rawNodes[i]);
            }
        }
        public static double Linear(double x)
        { return x; }
        public static double Random(double x)
        { 
            Random rnd = new Random(); 
            return rnd.NextDouble();
        }
        public static double Cubic(double x)
        { return x * (x - 1) * (x - 2) + 2; }

        public bool Save(string filename)
        {
            StreamWriter? file = null;
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IncludeFields = true
            };
            file = new StreamWriter(filename, false);
            string strLeftEnd = JsonSerializer.Serialize<double>(this.leftEnd, options);
            string strRightEnd = JsonSerializer.Serialize<double>(this.rightEnd, options);
            string strNRawNodes = JsonSerializer.Serialize<int>(this.nRawNodes, options);
            string strIsUniforms = JsonSerializer.Serialize<bool>(this.isUniform, options);
            string strRawNodes = JsonSerializer.Serialize<double[]>(this.rawNodes, options);
            string strRawValues = JsonSerializer.Serialize<double[]>(this.rawValues, options);
            string strFRaw;
            if (fRaw(0.5) == 0.5)
            {
                strFRaw = "1";
            }
            else if (fRaw(0.5) < 1)
            {
                strFRaw = "2";
            }
            else
            {
                strFRaw = "3";
            }
            file.WriteLine(strLeftEnd);
            file.WriteLine(strRightEnd);
            file.WriteLine(strNRawNodes);
            file.WriteLine(strIsUniforms);
            file.WriteLine(strRawNodes);
            file.WriteLine(strRawValues);
            file.WriteLine(strFRaw);
            if (file != null) file.Close();
            return true;
        }

        public static bool Load(string filename, ref RawData rawData)
        {
            StreamReader? file = null;
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IncludeFields = true
            };
            file = new StreamReader(filename);
            string? strLeftEnd = file.ReadLine();
            string? strRightEnd = file.ReadLine();
            string? strNRawNodes = file.ReadLine();
            string? strIsUniform = file.ReadLine();
            string? strRawNodes = file.ReadLine();
            string? strRawValues = file.ReadLine();
            string? strFRaw = file.ReadLine();
            rawData.leftEnd = JsonSerializer.Deserialize<double>(strLeftEnd, options);
            rawData.rightEnd = JsonSerializer.Deserialize<double>(strRightEnd, options);
            rawData.nRawNodes = JsonSerializer.Deserialize<int>(strNRawNodes, options);
            rawData.isUniform = JsonSerializer.Deserialize<bool>(strIsUniform, options);
            rawData.rawNodes = JsonSerializer.Deserialize<double[]>(strRawNodes, options);
            rawData.rawValues = JsonSerializer.Deserialize<double[]>(strRawValues, options);
            if (strFRaw == "1")
            {
                rawData.fRaw = RawData.Linear;
            }
            else if (strFRaw == "2")
            {
                rawData.fRaw = RawData.Random;
            }
            else
            {
                rawData.fRaw = RawData.Cubic;
            }
            if (file != null) file.Close();
            return true;
        }
    }
}
