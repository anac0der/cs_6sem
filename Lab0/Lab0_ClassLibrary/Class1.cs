using System;
using System.Numerics;
using System.Xml.Linq;
using System.Collections;

namespace Lab0_ClassLibrary
{
    delegate Complex FComplex(double x);
    public enum FuncEnum { Field, Const };
    public struct DataItem
    {
        public double x { get; set; }
        public Complex value { get; set; }

        public DataItem(double x, Complex value)
        {
            this.x = x;
            this.value = value;
        }

        public string ToLongString(string format)
        {
            return "x = " + x.ToString(format) + " value = " + value.ToString(format);
        }

        public override string ToString()
        {
            return $"x = {x} value = {value}";
        }
    }

    public abstract class V1Data
    {
        public static Complex Field(double x)
        {
            return new Complex(x * x * x, x * x * x);
        }

        public static Complex Const(double x)
        {
            return new Complex(x, 0);
        }
        public string ObjectID { get; set; }
        public DateTime date { get; set; }

        public V1Data(string ObjectID, DateTime date)
        {
            this.ObjectID = ObjectID;
            this.date = date;
        }

        public abstract double MaxMagnitude { get; }

        public abstract string ToLongString(string format);
        public override string ToString()
        {
            return $"ObjectID = {ObjectID}, date = {date.ToString()}";
        }
    }

    public class V1DataList : V1Data
    {
        public List<DataItem> Data { get; set; }
        public V1DataList(string ObjectID, DateTime date) : base(ObjectID, date)
        {
            this.Data = new List<DataItem>();
        }

        public bool Add(double x, Complex field)
        {
            bool isInList = false;
            for (int i = 0; i < Data.Count; ++i)
            {
                if (Data[i].x == x)
                {
                    isInList = true;
                    break;
                }
            }
            if (isInList)
            {
                return false;
            }
            Data.Add(new DataItem(x, field));
            return true;
        }

        public void AddDefaults(int a, int b, int nItems, FuncEnum Method)
        {
            FComplex F;
            if (Method == FuncEnum.Field)
            {
                F = Field;
            }
            else
            {
                F = Const;
            }
            Random rnd = new Random();
            for (int i = 0; i < nItems; ++i)
            {
                double x = (b - a) * rnd.NextDouble() + a;
                Add(x, F(x));
            }
        }

        public override double MaxMagnitude
        {
            get
            {
                double maxMagnitude = 0;
                for (int i = 0; i < Data.Count; ++i)
                {
                    if (Data[i].value.Magnitude > maxMagnitude)
                    {
                        maxMagnitude = Data[i].value.Magnitude;
                    }
                }
                return maxMagnitude;
            }
        }

        public override string ToString()
        {
            return $"Тип объекта: V1DataList, ObjectID = {ObjectID}, date = {date}.\n";
        }

        public override string ToLongString(string format)
        {
            string pattern = $"Тип объекта: V1DataList, ObjectID = {ObjectID}, date = {date}.\n";

            for (int i = 0; i < Data.Count; ++i)
            {
                pattern += $"x = {Data[i].x.ToString(format)} value = {Data[i].value.ToString(format)}\n";
            }
            return pattern;
        }
    }
}