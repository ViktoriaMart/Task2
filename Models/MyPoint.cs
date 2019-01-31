using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Models
{
    public class MyPoint: MyShape
    {
        public double X
        {
            get;
            set;
        }

        public double Y
        {
            get;
            set;
        }

        public const string pointName = "point";

        public MyPoint(double x, double y) : base(pointName, false)
        {
            X = x;
            Y = y;
        }
    }
}
