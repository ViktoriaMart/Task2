using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace Models
{
    public class MyPolygon: MyShape
    {
        public List<Point> PointList
        {
            get;
            set;
        }

        public MyPolygon()
        {
        }

        public MyPolygon(string name, List<Point> pointList, bool isChose = false) : base(name, isChose)
        {
            PointList = new List<Point>();
            foreach (var item in pointList)
            {
                PointList.Add(item);
            }
        }
    }
}
