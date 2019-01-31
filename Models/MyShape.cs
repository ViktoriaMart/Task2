using System;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models
{
    public abstract class MyShape : INotifyPropertyChanged
    {
        bool isChoose;
        //Color color = Color.FromRgb(255, 255, 255);
        Color color = Color.FromArgb(0, 255, 255, 255);
        Point margin;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public MyShape()
        {
        }

        public MyShape(string name, bool isChoose)
        {
            Name = name;
            IsChoose = isChoose;
        }


        public string Name
        {
            get;
            set;
        }

        public bool IsChoose
        {
            get
            {
                return isChoose;
            }
            set
            {
                if (value != this.isChoose)
                {
                    this.isChoose = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                if (value != this.color)
                {
                    this.color = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Point Margin
        {
            get
            {
                return margin;
            }
            set
            {
                if (value != this.margin)
                {
                    this.margin = value;
                    NotifyPropertyChanged();
                }
            }
        }

    }
}
