using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;
using BUS;
using Models;
using System.Windows.Input;
using System.Collections.Specialized;
using Microsoft.Win32;
using System.ComponentModel;

namespace Paint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ShapeBUS bus;
        Action action = null;
        bool isOpen;

        public MainWindow()
        {
            InitializeComponent();
            bus = new ShapeBUS();

            bus.Shapes.CollectionChanged += Shapes_CollectionChanged;
            ShapesListMenu.ItemsSource = bus.Shapes;
            ContextMenuItems.ItemsSource = bus.Shapes;

            ShapesListMenu.IsEnabled = false;

            CommandBindings.Add(new CommandBinding(ApplicationCommands.New, NewMenuItem_Click, (sender, e)=> { e.CanExecute = true; }));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, OpenMenuItem_Click, (sender, e) => { e.CanExecute = true; }));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, SaveMenuItem_Click, (sender, e) => { e.CanExecute = true; }));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.SaveAs, SaveAsMenuItem_Click, (sender, e) => { e.CanExecute = true; }));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, ExitMenuItem_Click, (sender, e) => { e.CanExecute = true; }));
        }

        private void Shapes_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Color")
            {
                switch (sender)
                {
                    case MyPolygon polygonShape:
                        (CanvasPaint.Children[bus.ChoosenShapeIndex] as Shape).Fill = new SolidColorBrush((sender as MyShape).Color);
                        break;
                    default:
                        break;
                }
            }
            else if (e.PropertyName == "Margin")
            {
                switch (sender)
                {
                    case MyPolygon polygonShape:
                        (CanvasPaint.Children[bus.ChoosenShapeIndex] as Shape).Margin = new Thickness((sender as MyShape).Margin.X, (sender as MyShape).Margin.Y, 0, 0);
                        break;
                    default:
                        break;
                }
            }
            else if (e.PropertyName == "IsChoose")
            {
                int strokeThickness = 1;
                if ((sender as MyShape).IsChoose)
                {
                    strokeThickness = 2;
                }
                (CanvasPaint.Children[bus.ChoosenShapeIndex] as Shape).StrokeThickness = strokeThickness;
            }
            else if (e.PropertyName == "AddPoint")
            {
                ShapesListMenu.IsEnabled = true;
            }
        }


        private void Shapes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    (e.NewItems[0] as INotifyPropertyChanged).PropertyChanged += Shapes_PropertyChanged;
                    ShapesListMenu.IsEnabled = true;

                    switch (e.NewItems[0])
                    {
                        case MyPoint point:
                            CanvasPaint.Children.Add(NewShapeForCancas(point));
                            ShapesListMenu.IsEnabled = false;
                            break;
                        case MyPolygon polygon:
                            CanvasPaint.Children.Add(NewShapeForCancas(polygon));
                            break;
                        default:
                            break;
                    }

                    if (!(e.NewItems[0] is MyPoint) && !isOpen)
                    {
                        bus.ChooseShape((e.NewItems[0] as MyShape).Name);
                        bus.FillShape(GetColor());
                        bus.ClearChoose();
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    switch (e.OldItems[0])
                    {
                        case MyPoint point:
                            var points = CanvasPaint.Children.OfType<Ellipse>().Where(p => p.Name == MyPoint.pointName).ToList();
                            foreach (var item in points)
                            {
                                CanvasPaint.Children.Remove(item);
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ShapesListMenu.IsEnabled = false;
                    action = null;
                    CanvasPaint.Children.Clear();
                    break;
            }
        }

        private void ShapeItem_Click(object sender, RoutedEventArgs e)
        {
            isOpen = false;
            bus.RemoveAllPoint();

            if (((MenuItem)sender).Name == "Pentagon")
            {
                action = bus.CreatePentagon;
            }
            else
            {
                action = null;
            }

            bus.ClearChoose();
            CanvasPaint.MouseMove -= CanvasContainer_MouseMove;
        }

        Color GetColor()
        {
            Window1 window1 = new Window1();
            if (window1.ShowDialog() == true)
            {
                return window1.SelectedColor;
            }
            else
            {
                return Color.FromRgb(255,255,255);
            }
        }

        private void CanvasPaint_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (action != null && e.LeftButton == MouseButtonState.Pressed)
            {
                bus.AddPoint(Mouse.GetPosition(CanvasPaint));
                action();
            }
        }

        private void MenuItem_Shapes_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            bus.ChooseShape(menuItem.Header.ToString());
            action = null;
            foreach (Shape item in CanvasPaint.Children)
            {
                item.MouseDown -= CanvasChildren_MouseDown;
            }
            CanvasPaint.Children[bus.ChoosenShapeIndex].MouseDown += CanvasChildren_MouseDown;
        }

        private void CanvasChildren_MouseDown(object sender, MouseButtonEventArgs e)
        {
            bus.SetShapeMarginAndStartMovePoint(Mouse.GetPosition(CanvasPaint));
            CanvasPaint.MouseMove += CanvasContainer_MouseMove;
        }


        private void CanvasContainer_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                bus.MoveShape(Mouse.GetPosition(CanvasPaint));
            }
            if (e.LeftButton == MouseButtonState.Released)
            {
                CanvasPaint.MouseMove -= CanvasContainer_MouseMove;
            }
        }

        private void NewMenuItem_Click(object sender, ExecutedRoutedEventArgs e)
        {
            bus.NewCanvas();
        }

        private void SaveMenuItem_Click(object sender, ExecutedRoutedEventArgs e)
        {
            if (bus.isNewCanvas)
            {
                SaveAsMenuItem_Click(sender, e);
            }
            else
            {
                bus.SaveShapes();
            }
        }
        private void SaveAsMenuItem_Click(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                FileName = "Untitled",
                DefaultExt = ".xaml",
                Filter = "Xmal documents (.xaml)|*.xaml"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                bus.SaveShapes(saveFileDialog.FileName);
            }
        }

        private void OpenMenuItem_Click(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                isOpen = true;
                bus.GetShapes(openFileDialog.FileName);
            }
        }

        private void ExitMenuItem_Click(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        Shape NewShapeForCancas(MyPolygon polygonShape)
        {
            Polygon polygon = new Polygon();
            foreach (var item in polygonShape.PointList)
            {
                polygon.Points.Add(item);
            }
            polygon.Stroke = Brushes.Black;
            polygon.Fill = new SolidColorBrush(polygonShape.Color);
            polygon.Margin = new Thickness(polygonShape.Margin.X, polygonShape.Margin.Y, 0, 0);

            return polygon;
        }

        Shape NewShapeForCancas(MyPoint pointShape)
        {
            Ellipse ellipse = new Ellipse
            {
                Fill = Brushes.Black,
                Height = 2,
                Width = 2,
                Name = pointShape.Name,
                Margin = new Thickness(pointShape.X, pointShape.Y, 0, 0)
            };

            return ellipse;
        }
    }
}
