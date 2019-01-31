using System;
using System.Collections.Generic;
using Models;
using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Linq;
using DAO;

namespace BUS
{
    public class ShapeBUS
    {
        MyShapesDAO shapesDAO;
        string filePath;
        Point marginShape;
        Point startMovePoint;

        public ShapeBUS()
        {
            shapesDAO = new MyShapesDAO();
            Shapes = new ObservableCollection<MyShape>();
        }

        public ObservableCollection<MyShape> Shapes
        {
            get;
            set;
        }

        public int ChoosenShapeIndex
        {
            get;
            set;
        } = -1;

        public bool isNewCanvas
        {
            get;
            set;
        } = true;

        public void CreatePentagon()
        {
            List<Point> points = getListPoints();
            if (points.Count == 5)
            {
                MyPolygon polygonShape = new MyPolygon(genereteName("Pentagon"), points);
                Shapes.Add(polygonShape);

                RemoveAllPoint();
            }
        }

        public void AddPoint(Point point)
        {
            Shapes.Add(new MyPoint(point.X, point.Y));
        }

        List<Point> getListPoints() 
        {
            List<Point> points = new List<Point>();

            foreach (var item in Shapes)
            {
                if (item is MyPoint pointShape)
                {
                    points.Add(new Point(pointShape.X, pointShape.Y));
                }
            }

            return points;
        }

        public void RemoveAllPoint() 
        {
            foreach (var item in Shapes.ToList())
            {
                if (item is MyPoint pointShape)
                {
                    Shapes.Remove(item);
                }
            }
        }

        string genereteName(string shapeType)
        {
            return shapeType + "_" + ((from polygon in Shapes where polygon.Name.StartsWith(shapeType) select polygon).Count() + 1);
        }

        public void ChooseShape(string shapeName)
        {
            ClearChoose();
            ChoosenShapeIndex = Shapes.IndexOf(Shapes.Where(shape => shape.Name == shapeName).First());

            Shapes[ChoosenShapeIndex].IsChoose = true;
        }

        public void ClearChoose()
        {
            foreach (var item in Shapes)
            {
                item.IsChoose = false;
            }
        }

        public void FillShape(Color color)
        {
            if (color != Color.FromRgb(255, 255, 255))
            {
                Shapes[ChoosenShapeIndex].Color = color;
            }
        }

        public void NewCanvas()
        {
            Shapes.Clear();
            filePath = "";
            isNewCanvas = true;
        }

        public void SaveShapes(string fileName = "")
        {
            RemoveAllPoint();
            if (isNewCanvas)
            {
                filePath = fileName;
                isNewCanvas = false;
            }
            if (fileName != "")
            {
                filePath = fileName;
            }
            ClearChoose();
            shapesDAO.saveShapes(filePath, Shapes);
        }

        public void GetShapes(string fileName)
        {
            isNewCanvas = false;
            filePath = fileName;
            ObservableCollection<MyShape> observableCollection = shapesDAO.getShapes(fileName);
            Shapes.Clear();
            foreach (var item in observableCollection)
            {
                Shapes.Add(item);
            }
            ChoosenShapeIndex = -1;
        }

        public void SetShapeMarginAndStartMovePoint(Point startMovePoint)
        {
            marginShape = Shapes[ChoosenShapeIndex].Margin;
            this.startMovePoint = startMovePoint;
        }

        public void MoveShape(Point mousePoint)
        {
            Point newMarginPoint = new Point(mousePoint.X - startMovePoint.X + marginShape.X, mousePoint.Y - startMovePoint.Y + marginShape.Y);
            Shapes[ChoosenShapeIndex].Margin = newMarginPoint;
        }
    }
}
