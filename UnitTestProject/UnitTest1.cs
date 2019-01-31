using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DAO;
using BUS;
using Models;
using System.IO;


namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        ShapeBUS shapeBUS;
        MyShapesDAO shapesDAO;

        public UnitTest1()
        {

            shapesDAO = new MyShapesDAO();
        }

        [TestMethod]
        public void AddPointTest()
        {
            shapeBUS = new ShapeBUS();
            Point expectedPoint = new Point(5, 5);
            shapeBUS.AddPoint(expectedPoint);

            Point actualPoint = new Point((shapeBUS.Shapes[0] as MyPoint).X, (shapeBUS.Shapes[0] as MyPoint).Y);

            Assert.AreEqual(expectedPoint, actualPoint);
        }

        [TestMethod]
        public void CreatePentagonTest_With5Points()
        {
            shapeBUS = new ShapeBUS();
            shapeBUS.AddPoint(new Point(0, 0));
            shapeBUS.AddPoint(new Point(5, 5));
            shapeBUS.AddPoint(new Point(6, 5));
            shapeBUS.AddPoint(new Point(9, 5));
            shapeBUS.AddPoint(new Point(15, 5));

            shapeBUS.CreatePentagon();
        }

        [TestMethod]
        public void CreatePentagonTest_With1Points()
        {
            shapeBUS = new ShapeBUS();
            shapeBUS.AddPoint(new Point(0, 0));

            shapeBUS.CreatePentagon();
        }


        [TestMethod]
        public void RemoveAllPointTest()
        {
            shapeBUS = new ShapeBUS();
            shapeBUS.AddPoint(new Point(0, 0));
            shapeBUS.AddPoint(new Point(5, 6));
            shapeBUS.AddPoint(new Point(4, 0));

            shapeBUS.RemoveAllPoint();

            int expected = 0;
            int actual = shapeBUS.Shapes.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ChooseShapeTest()
        {
            shapeBUS = new ShapeBUS();
            shapeBUS.AddPoint(new Point(0, 0));
            shapeBUS.AddPoint(new Point(5, 5));
            shapeBUS.AddPoint(new Point(6, 5));
            shapeBUS.AddPoint(new Point(9, 5));
            shapeBUS.AddPoint(new Point(15, 5));

            shapeBUS.CreatePentagon();

            shapeBUS.ChooseShape("Pentagon_1");

            int expected = 0;
            int actual = shapeBUS.ChoosenShapeIndex;

            Assert.AreEqual(expected, actual);
        }



        [TestMethod]
        public void FillShapeTest1()
        {
            shapeBUS = new ShapeBUS();
            shapeBUS.AddPoint(new Point(0, 0));
            shapeBUS.AddPoint(new Point(5, 5));
            shapeBUS.AddPoint(new Point(6, 5));
            shapeBUS.AddPoint(new Point(9, 5));
            shapeBUS.AddPoint(new Point(15, 5));

            shapeBUS.CreatePentagon();

            shapeBUS.ChooseShape("Pentagon_1");

            Color color = Color.FromRgb(0, 0, 0);
            shapeBUS.FillShape(color);
        }

        [TestMethod]
        public void FillShapeTest2()
        {
            shapeBUS = new ShapeBUS();
            shapeBUS.AddPoint(new Point(0, 0));
            shapeBUS.AddPoint(new Point(5, 5));
            shapeBUS.AddPoint(new Point(6, 5));
            shapeBUS.AddPoint(new Point(9, 5));
            shapeBUS.AddPoint(new Point(15, 5));

            shapeBUS.CreatePentagon();

            Color color = Color.FromRgb(255, 255, 255);
            shapeBUS.FillShape(color);
        }


        [TestMethod]
        public void NewCanvasTest1()
        {
            shapeBUS = new ShapeBUS();
            shapeBUS.AddPoint(new Point(0, 0));
            shapeBUS.AddPoint(new Point(5, 5));
            shapeBUS.AddPoint(new Point(6, 5));
            shapeBUS.AddPoint(new Point(9, 5));
            shapeBUS.AddPoint(new Point(15, 5));

            shapeBUS.CreatePentagon();

            Color color = Color.FromRgb(255, 255, 255);
            shapeBUS.FillShape(color);

            shapeBUS.NewCanvas();
        }
        [TestMethod]
        public void MoveShapeTest1()
        {
            shapeBUS = new ShapeBUS();
            shapeBUS.AddPoint(new Point(0, 0));
            shapeBUS.AddPoint(new Point(5, 5));
            shapeBUS.AddPoint(new Point(6, 5));
            shapeBUS.AddPoint(new Point(9, 5));
            shapeBUS.AddPoint(new Point(15, 5));

            shapeBUS.CreatePentagon();

            shapeBUS.ChooseShape("Pentagon_1");

            shapeBUS.SetShapeMarginAndStartMovePoint(new Point(0, 1));
            shapeBUS.MoveShape(new Point(2, 3));
        }

        [TestMethod]
        public void SaveShapesTest1()
        {
            shapeBUS = new ShapeBUS();
            shapeBUS.AddPoint(new Point(0, 0));
            shapeBUS.AddPoint(new Point(5, 5));
            shapeBUS.AddPoint(new Point(6, 5));
            shapeBUS.AddPoint(new Point(9, 5));
            shapeBUS.AddPoint(new Point(15, 5));

            shapeBUS.CreatePentagon();

            shapeBUS.SaveShapes("test.xaml");

            var fileText = File.ReadLines("test.xaml");
            Assert.IsTrue(fileText.ToString().Length > 1);
        }

        [TestMethod]
        public void GetShapesTest1()
        {
            shapeBUS = new ShapeBUS();
            shapeBUS.AddPoint(new Point(0, 0));
            shapeBUS.AddPoint(new Point(5, 5));
            shapeBUS.AddPoint(new Point(6, 5));
            shapeBUS.AddPoint(new Point(9, 5));
            shapeBUS.AddPoint(new Point(15, 5));

            shapeBUS.CreatePentagon();

            shapeBUS.GetShapes("testing.xaml");
        }

    }
}
